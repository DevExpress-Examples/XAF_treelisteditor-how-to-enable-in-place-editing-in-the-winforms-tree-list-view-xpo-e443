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
        protected override void OnActivated() {
            base.OnActivated();
            TreeListEditor treeListEditor = View.Editor as TreeListEditor;
            if (treeListEditor != null) {
                treeListEditor.AllowEditChanged += treeListEditor_AllowEditChanged;
                if (treeListEditor.TreeList != null) {
                    UpdateEditableTreeList(treeListEditor);
                    SubscribeToControlEvents(treeListEditor.TreeList);
                }
                treeListEditor.ControlsCreated += treeListEditor_ControlsCreated;
            }
        }
        void treeListEditor_ControlsCreated(object sender, EventArgs e) {
            TreeListEditor treeListEditor = (TreeListEditor)sender;
            UpdateEditableTreeList(treeListEditor);
            SubscribeToControlEvents(treeListEditor.TreeList);
        }
        private void SubscribeToControlEvents(TreeList treeList) {
            treeList.CellValueChanged += treeList_CellValueChanged;
            treeList.ShownEditor += treeList_ShownEditor;
        }
        protected override void OnDeactivated() {
            TreeListEditor treeListEditor = View.Editor as TreeListEditor;
            if (treeListEditor != null) {
                treeListEditor.AllowEditChanged -= treeListEditor_AllowEditChanged;
                treeListEditor.ControlsCreated -= treeListEditor_ControlsCreated;
                ObjectTreeList treeList = (ObjectTreeList)treeListEditor.TreeList;
                if (treeList != null) {
                    treeList.CellValueChanged -= treeList_CellValueChanged;
                    treeList.ShownEditor -= treeList_ShownEditor;
                }
            }
            base.OnDeactivated();
        }
        private void UpdateEditableTreeList(TreeListEditor treeListEditor) {
            ObjectTreeList treeList = treeListEditor.TreeList as ObjectTreeList;
            if (treeList != null) {
                treeList.OptionsBehavior.Editable = treeListEditor.AllowEdit;
                foreach (RepositoryItem ri in treeList.RepositoryItems) {
                    ri.ReadOnly = !treeListEditor.AllowEdit;
                }
                foreach (TreeListColumnWrapper columnWrapper in treeListEditor.Columns) {
                    IModelColumn modelColumn = View.Model.Columns[columnWrapper.PropertyName];
                    if (modelColumn != null)
                        columnWrapper.Column.OptionsColumn.AllowEdit = modelColumn.AllowEdit;
                }
                treeList.OptionsBehavior.ImmediateEditor = true;
            }
        }
        void treeListEditor_AllowEditChanged(object sender, EventArgs e) {
            UpdateEditableTreeList((TreeListEditor)sender);
        }
        private void treeList_ShownEditor(object sender, EventArgs e) {
            ObjectTreeList treeList = (ObjectTreeList)sender;
            IGridInplaceEdit activeEditor = treeList.ActiveEditor as IGridInplaceEdit;
            if (activeEditor != null && treeList.FocusedObject is IXPSimpleObject) {
                activeEditor.GridEditingObject = treeList.FocusedObject;
            }
        }
        private void treeList_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            if (!e.ChangedByUser)
                return;
            ObjectTreeList treeList = (ObjectTreeList)sender;
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