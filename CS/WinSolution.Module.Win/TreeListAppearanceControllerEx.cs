using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraTreeList;

namespace WinSolution.Module.Win {
    public class TreeListAppearanceControllerEx : TreeListAppearanceController {
        protected override void OnTreeListChanged() {
            base.OnTreeListChanged();
            if (base.Active.ResultValue && base.View != null && base.View.Editor != null && base.View.Editor is TreeListEditor && ((TreeListEditor)base.View.Editor).TreeList != null) {
                ((TreeListEditor)base.View.Editor).TreeList.ShowingEditor += new System.ComponentModel.CancelEventHandler(control_ShowingEditor);
            }
        }
        void control_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e) {
            TreeList tl = (TreeList)sender;
            ObjectTreeListNode node = tl.FocusedNode as ObjectTreeListNode;
            if (node == null) return;
            this.OnCustomizeAppearance(new CustomizeAppearanceEventArgs(tl.FocusedColumn.FieldName, "ViewItem", new GridViewCancelEventArgsAppearanceAdapter(null, e), node.Object, null));
        }
    }
}
