Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.TreeListEditors.Win
Imports DevExpress.ExpressApp.Win.Controls
Imports DevExpress.ExpressApp.Win.Editors
Imports DevExpress.XtraTreeList

Namespace WinSolution.Module.Win
    Public Class TreeListAppearanceControllerEx
        Inherits TreeListAppearanceController

        Protected Overrides Sub OnTreeListChanged()
            MyBase.OnTreeListChanged()
            If MyBase.Active.ResultValue AndAlso MyBase.View IsNot Nothing AndAlso MyBase.View.Editor IsNot Nothing AndAlso TypeOf MyBase.View.Editor Is TreeListEditor AndAlso CType(MyBase.View.Editor, TreeListEditor).TreeList IsNot Nothing Then
                AddHandler CType(View.Editor, TreeListEditor).TreeList.ShowingEditor, AddressOf control_ShowingEditor
            End If
        End Sub
        Private Sub control_ShowingEditor(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
            Dim tl As TreeList = DirectCast(sender, TreeList)
            Dim node As ObjectTreeListNode = TryCast(tl.FocusedNode, ObjectTreeListNode)
            If node Is Nothing Then
                Return
            End If
            Me.OnCustomizeAppearance(New CustomizeAppearanceEventArgs(tl.FocusedColumn.FieldName, "ViewItem", New GridViewCancelEventArgsAppearanceAdapter(Nothing, e), node.Object, Nothing))
        End Sub
    End Class
End Namespace
