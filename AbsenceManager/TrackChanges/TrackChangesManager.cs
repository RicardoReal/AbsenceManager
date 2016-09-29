using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbsenceManager.AppUtils;
using AbsenceManager.TrackChanges.ConcreteEntityTrackers;

namespace AbsenceManager.TrackChanges
{
    public class TrackChangesManager
    {
        private static Dictionary<Type, IEntityTrackChanges> _changesDictionary;

        public static void updateTrackChanges(TrackStruct entryStr, AM_Entities ent)
        {
            try
            {
                if (_changesDictionary.ContainsKey(entryStr.entry.Entity.GetType()))
                {
                    User u = GeneralHelpers.GetCurrentUser(ent);
                    _changesDictionary[entryStr.entry.Entity.GetType()].processChanges(entryStr, u);
                }
            }
            catch (Exception e)
            {
                Utils.WriteErrorLog(e.Message, e.InnerException, e.StackTrace, "TrackChangesManager");
            }
        }

        public static void fillTrackChangesDictionary()
        {
            _changesDictionary = new Dictionary<Type, IEntityTrackChanges>();
            _changesDictionary.Add(typeof(User), new UserTracker());
            _changesDictionary.Add(typeof(HorasFuncionario), new HorasFuncionarioTracker());
            _changesDictionary.Add(typeof(FuncionarioHorasFuncionario), new FuncionarioHorasFuncionarioTracker());
            _changesDictionary.Add(typeof(AusenciaFuncionario), new AusenciaFuncionarioTracker());
            _changesDictionary.Add(typeof(FeriasFuncionario), new FeriasFuncionarioTracker());
        }
    }
}