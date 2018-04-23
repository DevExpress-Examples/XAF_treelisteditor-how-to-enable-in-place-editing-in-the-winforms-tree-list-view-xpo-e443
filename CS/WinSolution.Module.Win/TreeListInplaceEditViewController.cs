using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.XtraTreeList;
using DevExpress.XtraEditors.Repository;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Win.Core;

namespace WinSolution.Module.Win {
    public class TreeListInplaceEditViewController : ViewController<ListView> {
        private ObjectTreeList treeList;
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            TreeListEditor treeListEditor = View.Editor as TreeListEditor;
            if (treeListEditor != null) {
                treeList = (ObjectTreeList)treeListEditor.TreeList;
                foreach (RepositoryItem ri in treeList.RepositoryItems)
                    ri.ReadOnly = false;
                treeList.CellValueChanged += treeList_CellValueChanged;
                treeList.ShownEditor += treeList_ShownEditor;
                treeList.OptionsBehavior.Editable = true;
                treeList.OptionsView.EnableAppearanceEvenRow = true;
                treeList.OptionsView.EnableAppearanceOddRow = true;
                treeList.OptionsBehavior.ImmediateEditor = false;
            }
        }
        protected override void OnDeactivating() {
            if (treeList != null) {
                treeList.CellValueChanged -= treeList_CellValueChanged;
                treeList.ShownEditor -= treeList_ShownEditor;
            }
            base.OnDeactivating();
        }
        private void treeList_ShownEditor(object sender, EventArgs e) {
            IGridInplaceEdit activeEditor = treeList.ActiveEditor as IGridInplaceEdit;
            if (activeEditor != null && treeList.FocusedObject is IXPSimpleObject) {
                activeEditor.GridEditingObject = treeList.FocusedObject;
            }
        }
        private void treeList_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            object newValue = e.Value;
            if (e.Value is IXPSimpleObject)
                newValue = ObjectSpace.GetObject(e.Value);
            ReflectionHelper.SetMemberValue(treeList.FocusedObject, e.Column.FieldName, newValue);
            ObjectSpace.CommitChanges();
        }
    }
}