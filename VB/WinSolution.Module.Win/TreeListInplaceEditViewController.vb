Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.TreeListEditors.Win
Imports DevExpress.ExpressApp.Win.Controls
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.Persistent.Base
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Win.Core

Namespace WinSolution.Module.Win
	Public Class TreeListInplaceEditViewController
		Inherits ViewController(Of ListView)
		Private treeList As ObjectTreeList
		Protected Overrides Overloads Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()
			Dim treeListEditor As TreeListEditor = TryCast(View.Editor, TreeListEditor)
			If treeListEditor IsNot Nothing Then
				treeList = CType(treeListEditor.TreeList, ObjectTreeList)
				For Each ri As RepositoryItem In treeList.RepositoryItems
					ri.ReadOnly = False
				Next ri
				AddHandler treeList.CellValueChanged, AddressOf treeList_CellValueChanged
				AddHandler treeList.ShownEditor, AddressOf treeList_ShownEditor
				treeList.OptionsBehavior.Editable = True
				treeList.OptionsView.EnableAppearanceEvenRow = True
				treeList.OptionsView.EnableAppearanceOddRow = True
				treeList.OptionsBehavior.ImmediateEditor = False
			End If
		End Sub
		Protected Overrides Overloads Sub OnDeactivating()
			If treeList IsNot Nothing Then
				RemoveHandler treeList.CellValueChanged, AddressOf treeList_CellValueChanged
				RemoveHandler treeList.ShownEditor, AddressOf treeList_ShownEditor
			End If
			MyBase.OnDeactivating()
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
			ReflectionHelper.SetMemberValue(treeList.FocusedObject, e.Column.FieldName, newValue)
			ObjectSpace.CommitChanges()
		End Sub
	End Class
End Namespace