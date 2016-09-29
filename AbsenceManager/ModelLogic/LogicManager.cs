using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using AbsenceManager.ModelLogic.EntitiesLogic;

namespace AbsenceManager.ModelLogic
{
    public class LogicManager
    {
        private static Dictionary<Type, IEntityLogic> logicDictionary;
        private static Dictionary<Type, IEntityLogic> logicDeletedDictionary;

        public static void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            if (logicDictionary == null) fillDictionary();

            Type entityType = entry.Entity.GetType();

            if (logicDictionary.ContainsKey(entityType))
            {
                logicDictionary[entityType].processEntity(entry, ent);
            }
        }

        public static void processDeletedEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            if (logicDeletedDictionary == null) fillDeleteDictionary();

            Type entityType = entry.Entity.GetType();

            if (logicDeletedDictionary.ContainsKey(entityType))
            {
                logicDeletedDictionary[entityType].processEntity(entry, ent);
            }
        }

        public static void fillDictionary()
        {
            logicDictionary = new Dictionary<Type, IEntityLogic>();
            logicDictionary.Add(typeof(Feriado), new FeriadoLogic());
            logicDictionary.Add(typeof(HorasFuncionario), new HorasFuncionarioLogic());
            logicDictionary.Add(typeof(FuncionarioHorasFuncionario), new FuncionarioHorasFuncionarioLogic());
            logicDictionary.Add(typeof(User), new UserLogic());
            logicDictionary.Add(typeof(Empresa), new EmpresaLogic());
            logicDictionary.Add(typeof(FeriasFuncionario), new FeriasFuncionarioLogic());
            logicDictionary.Add(typeof(AusenciaFuncionario), new AusenciaFuncionarioLogic());
        }

        public static void fillDeleteDictionary()
        {
            logicDeletedDictionary = new Dictionary<Type, IEntityLogic>();
            logicDeletedDictionary.Add(typeof(Departamento), new DepartamentoLogic());
            logicDeletedDictionary.Add(typeof(Role), new RoleLogic());
            logicDeletedDictionary.Add(typeof(HorasFuncionario), new HorasFuncionarioLogic());
            logicDeletedDictionary.Add(typeof(FuncionarioHorasFuncionario), new FuncionarioHorasFuncionarioLogic());
            logicDeletedDictionary.Add(typeof(AusenciaFuncionario), new AusenciaFuncionarioLogic());
        }
    }
}