using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace WinSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            Person p = new Person(Session);
            p.FirstName="Person1";
            ProjectGroup pg = new ProjectGroup(Session, "ProjectGroup1");
            pg.Person = p;
            Project pr = new Project(Session, "Project1");
            pr.Person = p;
            pg.Projects.Add(pr);
            ProjectArea pa = new ProjectArea(Session, "ProjectArea1");
            pa.Person = p;
            pr.ProjectAreas.Add(pa);
            p.Save();
            pa.Save();
            pr.Save();
            pg.Save();
        }
    }
}
