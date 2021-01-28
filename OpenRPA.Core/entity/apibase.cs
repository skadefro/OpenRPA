using OpenRPA.Interfaces;
using OpenRPA.Interfaces.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Core.entity
{
    public class apibase : ObservableObject, IBase
    {
        public string _id { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public string _type { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public string name { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public DateTime _modified { get { return GetProperty<DateTime>(); } set { SetProperty(value); } }
        public string _modifiedby { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public string _modifiedbyid { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public DateTime _created { get { return GetProperty<DateTime>(); } set { SetProperty(value); } }
        public string _createdby { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public string _createdbyid { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public ace[] _acl { get { return GetProperty<ace[]>(); } set { SetProperty(value); } }
        public string[] _encrypt { get { return GetProperty<string[]>(); } set { SetProperty(value); } }
        public long _version { get { return GetProperty<long>(); } set { SetProperty(value); } }
        Iace[] IBase._acl { get => _acl; set => _acl = value as ace[]; }
        public bool hasRight(Iapiuser user, ace_right bit)
        {
            if (_acl == null) return true;
            if (user == null) return true;
            var ace = _acl.Where(x => x._id == user._id).FirstOrDefault();
            if (ace != null) { if (ace.getBit((decimal)bit)) return true; }
            foreach (var role in user.roles)
            {
                ace = _acl.Where(x => x._id == role._id).FirstOrDefault();
                if (ace != null) { if (ace.getBit((decimal)bit)) return true; }
            }
            return false;
        }
        public bool hasRight(ITokenUser user, ace_right bit)
        {
            if (_acl == null) return true;
            if (user == null) return true;
            var ace = _acl.Where(x => x._id == user._id).FirstOrDefault();
            if (ace != null) { if (ace.getBit((decimal)bit)) return true; }
            foreach (var role in user.roles)
            {
                ace = _acl.Where(x => x._id == role._id).FirstOrDefault();
                if (ace != null) { if (ace.getBit((decimal)bit)) return true; }
            }
            return false;
        }
        public void AddRight(ITokenUser user, ace_right[] rights)
        {
            AddRight(user._id, user.name, rights);
        }
        public void AddRight(string _id, string name, ace_right[] rights)
        {
            if (_acl == null) _acl = new ace[] { };
            var ace = _acl.Where(x => x._id == _id).FirstOrDefault();
            if (ace == null) { ace = new ace(); _acl = _acl.Concat(new ace[] { ace }).ToArray(); ace._id = _id; ace.name = name; }
            if (rights != null && rights.Length > 0)
            {
                for (var bit = 0; bit < 10; bit++) ace.unsetBit((bit + 1));
                foreach (Interfaces.entity.ace_right bit in rights) ace.setBit((decimal)bit);
            }
        }
    }
}
