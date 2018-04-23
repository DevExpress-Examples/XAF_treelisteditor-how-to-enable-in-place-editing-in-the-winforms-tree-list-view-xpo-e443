using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraTreeList;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Core;
using DevExpress.XtraEditors.Repository;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.ExpressApp.TreeListEditors.Win;

namespace WinSolution.Module.Win {
    public class TreeListInplaceEditViewController : ViewController<ListView> {
        private ObjectTreeList treeList;
       protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            TreeListEditor treeListEditor = View.Editor as TreeListEditor;
            if (treeListEditor != null) {
                treeList = (ObjectTreeList)treeListEditor.TreeList;
                if (treeListEditor.AllowEdit) {
                    treeList.OptionsBehavior.Editable = true;
                    foreach (RepositoryItem ri in treeList.RepositoryItems)
                        ri.ReadOnly = false;
                    foreach (TreeListColumnWrapper columnWrapper in treeListEditor.Columns) {
                        IModelColumn modelColumn = View.Model.Columns[columnWrapper.PropertyName];
                        if (modelColumn != null)
                            columnWrapper.Column.OptionsColumn.AllowEdit = modelColumn.AllowEdit;
                    }
                    treeList.CellValueChanged += treeList_CellValueChanged;
                    treeList.ShownEditor += treeList_ShownEditor;
                    treeList.OptionsBehavior.ImmediateEditor = true;
                }
            }
        }
        protected override void OnDeactivated() {
            if (treeList != null) {
                treeList.CellValueChanged -= treeList_CellValueChanged;
                treeList.ShownEditor -= treeList_ShownEditor;
            }
            base.OnDeactivated();
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
            object focusedObject = treeList.FocusedObject;
            if (focusedObject != null) {
                IMemberInfo focusedColumnMemberInfo = ObjectSpace.TypesInfo.FindTypeInfo(focusedObject.GetType()).FindMember(e.Column.FieldName);
                if (focusedColumnMemberInfo != null)
                    focusedColumnMemberInfo.SetValue(focusedObject, Convert.ChangeType(newValue, focusedColumnMemberInfo.MemberType));
            }
        }
    }
}