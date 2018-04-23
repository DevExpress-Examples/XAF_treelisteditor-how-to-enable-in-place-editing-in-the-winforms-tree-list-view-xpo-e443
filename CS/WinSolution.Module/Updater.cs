using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace WinSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(DevExpress.ExpressApp.IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            Person p1 = new Person(Session);
            p1.FirstName="Person1";
            Person p2 = new Person(Session);
            p2.FirstName = "Person2";
            ProjectGroup pg = new ProjectGroup(Session, "ProjectGroup1");
            pg.Person = p1;
            Project pr = new Project(Session, "Project1");
            pr.Person = p1;
            pg.Projects.Add(pr);
            ProjectArea pa = new ProjectArea(Session, "ProjectArea1");
            pa.Person = p1;
            pr.ProjectAreas.Add(pa);
            p1.Save();
            p2.Save();
            pa.Save();
            pr.Save();
            pg.Save();
        }
    }
}
