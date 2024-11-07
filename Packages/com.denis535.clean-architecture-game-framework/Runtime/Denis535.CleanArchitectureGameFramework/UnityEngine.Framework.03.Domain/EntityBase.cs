#nullable enable
namespace UnityEngine.Framework.Domain {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    [DefaultExecutionOrder( 100 )]
    public abstract class EntityBase : MonoBehaviour {

        // Awake
        protected virtual void Awake() {
        }
        protected virtual void OnDestroy() {
        }

    }
    public abstract class ActorBase : EntityBase {
    }
    public abstract class ThingBase : EntityBase {
    }
    public abstract class VehicleBase : EntityBase {
    }
    public interface IDamageable {
        void Damage(DamageInfo info);
    }
    public static class IDamageableExtensions {

        public static bool TryDamage(this Transform transform, DamageInfo info, Predicate<IDamageable> predicate, [NotNullWhen( true )] out IDamageable? damageable) {
            var damageable_ = transform.root.GetComponent<IDamageable>();
            if (damageable_ != null && predicate( damageable_ )) {
                damageable_.Damage( info );
                damageable = damageable_;
                return true;
            }
            damageable = null;
            return false;
        }

    }
    public abstract record DamageInfo(float Damage);
    public record HitDamageInfo(float Damage, Vector3 Point, Vector3 Direction, Vector3 Original, ThingBase Thing, ActorBase Actor, PlayerBase? Player) : DamageInfo( Damage );
    public record ExplosionDamageInfo(float Damage, Vector3 Point, Vector3 Direction, Vector3 Original, ThingBase Thing, ActorBase Actor, PlayerBase? Player) : DamageInfo( Damage );
    public record VehicleDamageInfo(float Damage, Vector3 Point, Vector3 Direction, Vector3 Original, VehicleBase Vehicle, PlayerBase? Player) : DamageInfo( Damage );
    public record KillZoneDamageInfo(float Damage) : DamageInfo( Damage );
}
