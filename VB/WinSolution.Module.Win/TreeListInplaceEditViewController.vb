Imports System
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.XtraTreeList
Imports DevExpress.ExpressApp.DC
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Win.Core
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.ExpressApp.Win.Controls
Imports DevExpress.ExpressApp.TreeListEditors.Win

Namespace WinSolution.Module.Win

    Public Class TreeListInplaceEditViewController
        Inherits ViewController(Of ListView)

        Protected Overrides Sub OnActivated()
            MyBase.OnActivated()
            Dim treeListEditor As TreeListEditor = TryCast(View.Editor, TreeListEditor)
            If treeListEditor IsNot Nothing Then
                AddHandler treeListEditor.AllowEditChanged, AddressOf treeListEditor_AllowEditChanged
                If treeListEditor.TreeList IsNot Nothing Then
                    UpdateEditableTreeList(treeListEditor)
                    SubscribeToControlEvents(treeListEditor.TreeList)
                End If
                AddHandler treeListEditor.ControlsCreated, AddressOf treeListEditor_ControlsCreated
            End If
        End Sub
        Private Sub treeListEditor_ControlsCreated(ByVal sender As Object, ByVal e As EventArgs)
            Dim treeListEditor As TreeListEditor = DirectCast(sender, TreeListEditor)
            UpdateEditableTreeList(treeListEditor)
            SubscribeToControlEvents(treeListEditor.TreeList)
        End Sub
        Private Sub SubscribeToControlEvents(ByVal treeList As TreeList)
            AddHandler treeList.CellValueChanged, AddressOf treeList_CellValueChanged
            AddHandler treeList.ShownEditor, AddressOf treeList_ShownEditor
        End Sub
        Protected Overrides Sub OnDeactivated()
            Dim treeListEditor As TreeListEditor = TryCast(View.Editor, TreeListEditor)
            If treeListEditor IsNot Nothing Then
                RemoveHandler treeListEditor.AllowEditChanged, AddressOf treeListEditor_AllowEditChanged
                RemoveHandler treeListEditor.ControlsCreated, AddressOf treeListEditor_ControlsCreated
                Dim treeList As ObjectTreeList = CType(treeListEditor.TreeList, ObjectTreeList)
                If treeList IsNot Nothing Then
                    RemoveHandler treeList.CellValueChanged, AddressOf treeList_CellValueChanged
                    RemoveHandler treeList.ShownEditor, AddressOf treeList_ShownEditor
                End If
            End If
            MyBase.OnDeactivated()
        End Sub
        Private Sub UpdateEditableTreeList(ByVal treeListEditor As TreeListEditor)
            Dim treeList As ObjectTreeList = TryCast(treeListEditor.TreeList, ObjectTreeList)
            If treeList IsNot Nothing Then
                treeList.OptionsBehavior.Editable = treeListEditor.AllowEdit
                For Each ri As RepositoryItem In treeList.RepositoryItems
                    ri.ReadOnly = Not treeListEditor.AllowEdit
                Next ri
                For Each columnWrapper As TreeListColumnWrapper In treeListEditor.Columns
                    Dim modelColumn As IModelColumn = View.Model.Columns.Item(columnWrapper.PropertyName)
                    If modelColumn IsNot Nothing Then
                        columnWrapper.Column.OptionsColumn.AllowEdit = modelColumn.AllowEdit
                    End If
                Next columnWrapper
                treeList.OptionsBehavior.ImmediateEditor = True
            End If
        End Sub
        Private Sub treeListEditor_AllowEditChanged(ByVal sender As Object, ByVal e As EventArgs)
            UpdateEditableTreeList(DirectCast(sender, TreeListEditor))
        End Sub
        Private Sub treeList_ShownEditor(ByVal sender As Object, ByVal e As EventArgs)
            Dim treeList As ObjectTreeList = DirectCast(sender, ObjectTreeList)
            Dim activeEditor As IGridInplaceEdit = TryCast(treeList.ActiveEditor, IGridInplaceEdit)
            If activeEditor IsNot Nothing AndAlso TypeOf treeList.FocusedObject Is IXPSimpleObject Then
                activeEditor.GridEditingObject = treeList.FocusedObject
            End If
        End Sub
        Private Sub treeList_CellValueChanged(ByVal sender As Object, ByVal e As CellValueChangedEventArgs)
            If Not e.ChangedByUser Then
                Return
            End If
            Dim treeList As ObjectTreeList = DirectCast(sender, ObjectTreeList)
            Dim newValue As Object = e.Value
            If TypeOf e.Value Is IXPSimpleObject Then
                newValue = ObjectSpace.GetObject(e.Value)
            End If
            Dim focusedObject As Object = treeList.FocusedObject
            If focusedObject IsNot Nothing Then
                Dim focusedColumnMemberInfo As IMemberInfo = ObjectSpace.TypesInfo.FindTypeInfo(focusedObject.GetType()).FindMember(e.Column.FieldName)
                If focusedColumnMemberInfo IsNot Nothing Then
                    focusedColumnMemberInfo.SetValue(focusedObject, Convert.ChangeType(newValue, focusedColumnMemberInfo.MemberType))
                End If
            End If
        End Sub
    End Class
End Namespace