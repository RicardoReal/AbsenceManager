using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace AbsenceManager.TrackChanges.ConcreteEntityTrackers
{
    public class UserTracker : IEntityTrackChanges
    {
        public void processChanges(TrackStruct entryStr, User u)
        {
            if (entryStr.entryState == System.Data.EntityState.Added)
                processAdded(entryStr.entry, u);
            else if (entryStr.entryState == System.Data.EntityState.Modified)
                processModified(entryStr.entry, u);
        }

        private void processAdded(ObjectStateEntry entry, User u)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                string[] addedFields = new string[] { "NrFuncionario", "Nome", "UserName", "Password", "IsAdmin", "RoleID", "DepartamentoID", "EmpresaID", "NrDiasFerias", "Activo", "NrFardamento", "Sapato", "Calça", "Polo", "Casaco", "Bata", "DataAdmissao", "Morada", "Telefone", "Email", "DataNascimento", "CC", "ValidadeCC", "NIF" };
                string[] fields;
                User newUser = (User)entry.Entity;

                foreach (String field in addedFields)
                {
                    fields = GetSingleFieldValue(ent, newUser, field);

                    if (fields != null && !String.IsNullOrEmpty(fields[1]))
                    {
                        ent.TrackUsers.AddObject
                            (
                                new TrackUser(
                                    newUser.ID,
                                    newUser.Nome,
                                    newUser.NrFuncionario,
                                    fields[0],
                                    "",
                                    fields[1],
                                    u.Nome,
                                    DateTime.Now)
                             );
                    }
                }

                ent.SaveChanges();
            }
        }

        private void processModified(ObjectStateEntry entry, User u)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                string oldValue, newValue, fieldName;

                User newUser = (User)entry.Entity;
                User oldUser = (from f in ent.Users where f.ID == newUser.ID select f).FirstOrDefault();
                string[] fields;

                IEnumerable<string> modifiedFields = entry.GetModifiedProperties();

                foreach (string mf in modifiedFields)
                {
                    fields = GetFieldValue(ent, newUser, oldUser, mf);
                    if (fields != null)
                    {
                        fieldName = fields[0];
                        newValue = fields[1];
                        oldValue = fields[2];

                        if (oldValue != newValue)
                        {
                            ent.TrackUsers.AddObject
                            (
                                new TrackUser(
                                    oldUser.ID,
                                    oldUser.Nome,
                                    oldUser.NrFuncionario,
                                    fieldName,
                                    oldValue,
                                    newValue,
                                    u.Nome,
                                    DateTime.Now)
                            );
                        }
                    }
                }
                ent.SaveChanges();
            }
        }

        private string[] GetFieldValue(AM_Entities ent, User newUser, User oldUser, string field)
        {
            switch (field)
            {
                case "NrFuncionario":
                    return new string[] { "Nr. Func.", newUser.NrFuncionario.ToString(), oldUser.NrFuncionario.ToString() };
                case "Nome":
                    return new string[] { "Nome", newUser.Nome, oldUser.Nome };
                case "UserName":
                    return new string[] { "UserName", newUser.UserName, oldUser.UserName };
                case "Password":
                    return new string[] { "Password", "******", "******" };
                case "IsAdmin":
                    return new string[] { "Is Admin", newUser.IsAdmin ? "Sim" : "Não", oldUser.IsAdmin ? "Sim" : "Não" };
                case "RoleID":
                    string newRole = (from rle in ent.Roles where rle.ID == newUser.RoleID select rle.Role1).FirstOrDefault();
                    return new string[] { "Role", newRole };
                case "DepartamentoID":
                    string newDpt = (from dpt in ent.Departamentoes where dpt.ID == newUser.DepartamentoID select dpt.Nome).FirstOrDefault();
                    string oldDpt = (from dpt in ent.Departamentoes where dpt.ID == oldUser.DepartamentoID select dpt.Nome).FirstOrDefault();
                    return new string[] { "Departamento", newDpt, oldDpt };
                case "EmpresaID":
                    string newEmp = (from emp in ent.Empresas where emp.ID == newUser.EmpresaID select emp.Nome).FirstOrDefault();
                    string oldEmp = (from emp in ent.Empresas where emp.ID == oldUser.EmpresaID select emp.Nome).FirstOrDefault();
                    return new string[] { "Empresa", newEmp, oldEmp };
                case "NrDiasFerias":
                    return new string[] { "Nr. Dias Férias", newUser.NrDiasFerias.ToString(), oldUser.NrDiasFerias.ToString() };
                case "CustoHora":
                    return new string[] { "Custo Hora", newUser.CustoHora.ToString(), oldUser.CustoHora.ToString() };
                case "Compensacoes":
                    return new string[] { "Compensações", newUser.Compensacoes.ToString(), oldUser.Compensacoes.ToString() };
                case "Activo":
                    return new string[] { "Activo", newUser.Activo ? "Sim" : "Não", oldUser.Activo ? "Sim" : "Não" };
                case "NrFardamento":
                    return new string[] { "Nr. Fardamento", newUser.NrFardamento.ToString(), oldUser.NrFardamento.ToString() };
                case "Sapato":
                    return new string[] { "Sapato", newUser.Sapato.ToString(), oldUser.Sapato.ToString() };
                case "Calça":
                    return new string[] { "Calça", newUser.Calça.ToString(), oldUser.Calça.ToString() };
                case "Polo":
                    return new string[] { "Polo", newUser.Polo.ToString(), oldUser.Polo.ToString() };
                case "Casaco":
                    return new string[] { "Casaco", newUser.Casaco.ToString(), oldUser.Casaco.ToString() };
                case "Bata":
                    return new string[] { "Bata", newUser.Bata.ToString(), oldUser.Bata.ToString() };
                case "NIF":
                    return new string[] { "NIF", newUser.NIF.ToString(), oldUser.NIF.ToString() };
                case "DataAdmissao":
                    return new string[] { "Data Admissão", newUser.DataAdmissao.Value.ToShortDateString(), oldUser.DataAdmissao.Value.ToShortDateString() };
                case "Morada":
                    return new string[] { "Morada", newUser.Morada.ToString(), oldUser.Morada.ToString() };
                case "Telefone":
                    return new string[] { "Telefone", newUser.Telefone.ToString(), oldUser.Telefone.ToString() };
                case "Email":
                    return new string[] { "Email", newUser.Email.ToString(), oldUser.Email.ToString() };
                case "DataNascimento":
                    return new string[] { "Data Nascimento", newUser.DataNascimento.Value.ToShortDateString(), oldUser.DataNascimento.Value.ToShortDateString() };
                default:
                    return null;
            }
        }

        private string[] GetSingleFieldValue(AM_Entities ent, User newUser, string field)
        {
            switch (field)
            {
                case "NrFuncionario":
                    return new string[] { "Nr. Func.", newUser.NrFuncionario.ToString() };
                case "Nome":
                    return new string[] { "Nome", newUser.Nome };
                case "UserName":
                    return new string[] { "UserName", newUser.UserName };
                case "Password":
                    return new string[] { "Password", newUser.Password };
                case "IsAdmin":
                    return new string[] { "Is Admin", newUser.IsAdmin ? "Sim" : "Não" };
                case "RoleID":
                    string newRole = (from rle in ent.Roles where rle.ID == newUser.RoleID select rle.Role1).FirstOrDefault();
                    return new string[] { "Role", newRole };
                case "DepartamentoID":
                    string newDpt = (from dpt in ent.Departamentoes where dpt.ID == newUser.DepartamentoID select dpt.Nome).FirstOrDefault();
                    return new string[] { "Departamento", newDpt };
                case "EmpresaID":
                    string newEmp = (from emp in ent.Empresas where emp.ID == newUser.EmpresaID select emp.Nome).FirstOrDefault();
                    return new string[] { "Empresa", newEmp };
                case "NrDiasFerias":
                    return new string[] { "Nr. Dias Férias", newUser.NrDiasFerias.ToString() };
                case "Activo":
                    return new string[] { "Activo", newUser.Activo ? "Sim" : "Não" };
                case "NrFardamento":
                    return new string[] { "Nr. Fardamento", newUser.NrFardamento.ToString() };
                case "Sapato":
                    return new string[] { "Sapato", newUser.Sapato.ToString() };
                case "Calça":
                    return new string[] { "Calça", newUser.Calça.ToString() };
                case "Polo":
                    return new string[] { "Polo", newUser.Polo.ToString() };
                case "Casaco":
                    return new string[] { "Casaco", newUser.Casaco.ToString() };
                case "Bata":
                    return new string[] { "Bata", newUser.Bata.ToString() };
                default:
                    return null;
            }
        }
    }
}