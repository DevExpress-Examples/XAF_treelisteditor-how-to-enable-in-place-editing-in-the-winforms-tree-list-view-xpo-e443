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
            Person p1 = ObjectSpace.CreateObject<Person>();
            p1.FirstName="Person1";
            Person p2 = ObjectSpace.CreateObject<Person>();
            p2.FirstName = "Person2";
            ProjectGroup pg = ObjectSpace.CreateObject<ProjectGroup>();
            pg.Name = "ProjectGroup1";
            pg.Person = p1;
            Project pr = ObjectSpace.CreateObject<Project>();
            pr.Name = "Project1";
            pr.Person = p1;
            pg.Projects.Add(pr);
            ProjectArea pa = ObjectSpace.CreateObject<ProjectArea>();
            pa.Name = "ProjectArea1";
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
