using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using System;
using System.Linq;
using System.Reflection;

namespace COPUnity
{
    public interface IWeapon
    {
        float BaseDamage { get; }
        float AttacksPerSec { get; }
        float Range { get; }
        string Description { get; }
    }

    public enum WeaponId
    {
        HockeyStick,
        Crowbar,
        HuntersKnife,
    }

    public class Weapon : IWeapon
    {
        public float BaseDamage { get; protected set; }
        public float AttacksPerSec { get; protected set; }
        public float Range { get; protected set; }
        public string Description { get { return string.Format("dmg: {0} aps: {1} rng: {2}", BaseDamage, AttacksPerSec, Range); }}
    }

    public class HockeyStick : Weapon 
    {
        public HockeyStick()
        {
            BaseDamage = 18f;
            AttacksPerSec = 1.75f;
            Range = 2.5f;
        }
    }

    public class Crowbar : Weapon 
    {
        public Crowbar()
        {
            BaseDamage = 23f;
            AttacksPerSec = 1.25f;
            Range = 2.4f;
        }
    }

    public class HuntersKnife : Weapon 
    {
        public HuntersKnife()
        {
            BaseDamage = 65f;
            AttacksPerSec = 1.20f;
            Range = 1.5f;
        }
    }

    public class WeaponBindings
    {
        public static void Bind(IInjectionBinder injectionBinder)
        {
            var type = typeof(IWeapon);
            var weaponTypes = Assembly.GetAssembly(typeof(IWeapon)).GetTypes()
                .Where(p => type.IsAssignableFrom(p) && p.BaseType == typeof(Weapon) && !p.IsAbstract);

            foreach(var t in weaponTypes)
            {
                var id = Enum.Parse(typeof(WeaponId), t.Name);
                injectionBinder.Bind<IWeapon>().To(t).ToName(id);
            }
        }
    }
}
