#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    [DefaultExecutionOrder( ExecutionOrder )]
    public abstract class EntityBase : MonoBehaviour {

        public const int ExecutionOrder = 100;

        // Awake
        protected abstract void Awake();
        protected abstract void OnDestroy();

        // For optimization purposes, we should not receive all messages.
        // Start
        //protected abstract void Start();
        //protected abstract void FixedUpdate();
        //protected abstract void Update();
        //protected abstract void LateUpdate();

    }
    // ActorBase
    public abstract class ActorBase : EntityBase {
    }
    // ThingBase
    public abstract class ThingBase : EntityBase {
    }
    // VehicleBase
    public abstract class VehicleBase : EntityBase {
    }
    // WorldBase
    public abstract class WorldBase : EntityBase {
    }
    // IDamageable
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
    // DamageInfo
    public abstract record DamageInfo(float Damage);
    public record HitDamageInfo(float Damage, Vector3 Point, Vector3 Direction, Vector3 Original, ThingBase Thing, ActorBase Actor, PlayerBase? Player) : DamageInfo( Damage );
    public record ExplosionDamageInfo(float Damage, Vector3 Point, Vector3 Direction, Vector3 Original, ThingBase Thing, ActorBase Actor, PlayerBase? Player) : DamageInfo( Damage );
    public record VehicleDamageInfo(float Damage, Vector3 Point, Vector3 Direction, Vector3 Original, VehicleBase Vehicle, PlayerBase? Player) : DamageInfo( Damage );
    public record KillZoneDamageInfo(float Damage) : DamageInfo( Damage );
}
