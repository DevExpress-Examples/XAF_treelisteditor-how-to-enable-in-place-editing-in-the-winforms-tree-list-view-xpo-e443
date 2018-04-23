Imports Microsoft.VisualBasic
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
		Private treeList As ObjectTreeList
	   	Protected Overrides Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()
			Dim treeListEditor As TreeListEditor = TryCast(View.Editor, TreeListEditor)
			If treeListEditor IsNot Nothing Then
				treeList = CType(treeListEditor.TreeList, ObjectTreeList)
				If treeListEditor.AllowEdit Then
					treeList.OptionsBehavior.Editable = True
					For Each ri As RepositoryItem In treeList.RepositoryItems
						ri.ReadOnly = False
					Next ri
					For Each columnWrapper As TreeListColumnWrapper In treeListEditor.Columns
						Dim modelColumn As IModelColumn = TryCast(View.Model.Columns.GetNode(columnWrapper.PropertyName), IModelColumn)
						If modelColumn IsNot Nothing Then
							columnWrapper.Column.OptionsColumn.AllowEdit = modelColumn.AllowEdit
						End If
					Next columnWrapper
					AddHandler treeList.CellValueChanged, AddressOf treeList_CellValueChanged
					AddHandler treeList.ShownEditor, AddressOf treeList_ShownEditor
					treeList.OptionsBehavior.ImmediateEditor = True
				End If
			End If
	   	End Sub
		Protected Overrides Sub OnDeactivated()
			If treeList IsNot Nothing Then
				RemoveHandler treeList.CellValueChanged, AddressOf treeList_CellValueChanged
				RemoveHandler treeList.ShownEditor, AddressOf treeList_ShownEditor
			End If
			MyBase.OnDeactivated()
		End Sub
		Private Sub treeList_ShownEditor(ByVal sender As Object, ByVal e As EventArgs)
			Dim activeEditor As IGridInplaceEdit = TryCast(treeList.ActiveEditor, IGridInplaceEdit)
			If activeEditor IsNot Nothing AndAlso TypeOf treeList.FocusedObject Is IXPSimpleObject Then
				activeEditor.GridEditingObject = treeList.FocusedObject
			End If
		End Sub
		Private Sub treeList_CellValueChanged(ByVal sender As Object, ByVal e As CellValueChangedEventArgs)
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