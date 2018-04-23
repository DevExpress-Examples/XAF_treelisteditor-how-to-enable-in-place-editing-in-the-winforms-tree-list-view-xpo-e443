using System;

using DevExpress.Xpo;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Data.Filtering;

namespace WinSolution.Module {
    [NavigationItem]
    public abstract class Category : BaseObject, ITreeNode {
        private string name;
        protected abstract ITreeNode Parent {
            get;
        }
        protected abstract IBindingList Children {
            get;
        }
        public Category(Session session) : base(session) { }
        public string Name {
            get {
                return name;
            }
            set {
                SetPropertyValue("Name", ref name, value);
            }
        }
        private Person _person;
        public Person Person {
            get { return _person; }
            set { SetPropertyValue("Person", ref _person, value); }
        }
        #region ITreeNode
        IBindingList ITreeNode.Children {
            get {
                return Children;
            }
        }
        string ITreeNode.Name {
            get {
                return Name;
            }
        }
        ITreeNode ITreeNode.Parent {
            get {
                return Parent;
            }
        }
        #endregion
    }

    public class ProjectGroup : Category {
        protected override ITreeNode Parent {
            get {
                return null;
            }
        }
        protected override IBindingList Children {
            get {
                return Projects;
            }
        }
        public ProjectGroup(Session session) : base(session) { }
        public ProjectGroup(Session session, string name)
            : base(session) {
            this.Name = name;
        }
        [Association("ProjectGroup-Projects"), Aggregated]
        public XPCollection<Project> Projects {
            get {
                return GetCollection<Project>("Projects");
            }
        }
    }

    public class Project : Category {
        private ProjectGroup projectGroup;
        protected override ITreeNode Parent {
            get {
                return projectGroup;
            }
        }
        protected override IBindingList Children {
            get {
                return ProjectAreas;
            }
        }
        public Project(Session session) : base(session) { }
        public Project(Session session, string name)
            : base(session) {
            this.Name = name;
        }
        [Association("ProjectGroup-Projects")]
        public ProjectGroup ProjectGroup {
            get {
                return projectGroup;
            }
            set {
                projectGroup = value;
                SetPropertyValue("ProjectGroup", ref projectGroup, value);
            }
        }
        [Association("Project-ProjectAreas"), Aggregated]
        public XPCollection<ProjectArea> ProjectAreas {
            get {
                return GetCollection<ProjectArea>("ProjectAreas");
            }
        }
    }

    public class ProjectArea : Category {
        private Project project;
        protected override ITreeNode Parent {
            get {
                return project;
            }
        }
        protected override IBindingList Children {
            get {
                return new BindingList<object>();
            }
        }
        public ProjectArea(Session session) : base(session) { }
        public ProjectArea(Session session, string name)
            : base(session) {
            this.Name = name;
        }
        [Association("Project-ProjectAreas")]
        public Project Project {
            get {
                return project;
            }
            set {
                project = value;
                SetPropertyValue("Project", ref project, value);
            }
        }
    }
}
