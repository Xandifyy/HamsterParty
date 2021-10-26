using UnityEngine;

namespace ExplosionForce2D
{
    public static class ExplosionForce2D
    {

        #region AddExplosionForce2D
        
        /// <summary>
        /// Applies a force to a Rigidbody2D that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the explosion.</param>
        /// <param name="explosionPosition">The center of the circle witch the explosion has its effect.</param>
        /// <param name="radius">Radius of the circle within witch the explosion has its effect.</param>
        /// <param name="modifyForceByDistance">If set to <c>true</c> force will be modified by distance from explosion center. \n Note that bodies with center of mass outside of the radius will not be affected with the explosion.</param>
        /// <param name="lookAtMovingDirection">If set to <c>true</c> bodies will be instantly rotated towards their moving direction.</param>
        /// <param name="lookAtAngleModifier">Modifies the angle of rotation.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddExplosionForce2D(this Rigidbody2D body, float force, Vector3 explosionPosition, float radius, bool modifyForceByDistance = true, bool lookAtMovingDirection = false, float lookAtAngleModifier = 0f, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - explosionPosition);
            float forceByDistanceModifier = 1.0f - (dir.magnitude / radius);
            Vector2 finalForce = dir.normalized * force;
            if (modifyForceByDistance)
            {
                if (forceByDistanceModifier > 0f)
                    finalForce *= forceByDistanceModifier;
                else
                    return;
            }
            if (body.attachedColliderCount > 0)
                body.AddForceAtPosition(finalForce, GetClosestPosition(body, explosionPosition), ForceMode2D); // find the closest point to explosionPosition and apply the force there
            else
                body.AddForce(finalForce, ForceMode2D);


            if (lookAtMovingDirection)
            {
                float predictedVelocity;
                if (modifyForceByDistance)
                    predictedVelocity = (force * forceByDistanceModifier / body.mass) * Time.fixedDeltaTime;
                else
                    predictedVelocity = (force / body.mass) * Time.fixedDeltaTime;
                Vector2 addingVelocity = dir.normalized * predictedVelocity;
                Vector2 nextStepVelocity = (addingVelocity + body.velocity);
                Vector3 lookAtPosition = body.transform.position + (Vector3)(nextStepVelocity.normalized);
                body.AddLookAt2D(lookAtPosition, lookAtAngleModifier);
            }
        }

        /// <summary>
        /// Applies a force to a Rigidbody2D that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the explosion.</param>
        /// <param name="explosionPosition">The center of the circle witch the explosion has its effect.</param>
        /// <param name="radius">Radius of the circle within witch the explosion has its effect.</param>
        /// <param name="modifyForceByDistance">If set to <c>true</c> force will be modified by distance from explosion center. \n Note that bodies with center of mass outside of the radius will not be affected with the explosion.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddExplosionForce2D(this Rigidbody2D body, float force, Vector3 explosionPosition, float radius, bool modifyForceByDistance = true, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - explosionPosition);
            Vector2 finalForce = dir.normalized * force;
            if (modifyForceByDistance)
            {
                float forceByDistanceModifier = 1.0f - (dir.magnitude / radius);
                if (forceByDistanceModifier > 0f)
                    finalForce *= forceByDistanceModifier;
                else
                    return;
            }
            if (body.attachedColliderCount > 0)
                body.AddForceAtPosition(finalForce, GetClosestPosition(body, explosionPosition), ForceMode2D); // find the closest point to explosionPosition and apply the force there
            else
                body.AddForce(finalForce, ForceMode2D);
        }

        /// <summary>
        /// Applies a force to a Rigidbody2D that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the explosion.</param>
        /// <param name="explosionPosition">The center of the circle witch the explosion has its effect.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddExplosionForce2D(this Rigidbody2D body, float force, Vector3 explosionPosition, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - explosionPosition);
            Vector2 finalForce = dir.normalized * force;
            body.AddForce(finalForce, ForceMode2D);
        }

        #endregion

        #region AddUpliftedExplosionForce2D

        /// <summary>
        /// Applies a force to a Rigidbody2D with uplift modifier that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the explosion.</param>
        /// <param name="explosionPosition">The center of the circle witch the explosion has its effect.</param>
        /// <param name="radius">Radius of the circle within witch the explosion has its effect.</param>
        /// <param name="upliftModifier">	Adjustment to the apparent position of the explosion to make it seem to lift objects.</param>
        /// <param name="modifyForceByDistance">If set to <c>true</c> force will be modified by distance from explosion center. \n Note that bodies with center of mass outside of the radius will not be affected with the explosion.</param>
        /// <param name="lookAtMovingDirection">If set to <c>true</c> bodies will be instantly rotated towards their moving direction.</param>
        /// <param name="lookAtAngleModifier">Modifies the angle of rotation.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddUpliftedExplosionForce2D(this Rigidbody2D body, float force, Vector3 explosionPosition, float radius, float upliftModifier = 1f, bool modifyForceByDistance = true, bool lookAtMovingDirection = false, float lookAtAngleModifier = 0f, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - explosionPosition);
            float forceByDistanceModifier = 1.0f - (dir.magnitude / radius);
            Vector2 baseForce = dir.normalized * force;
            if (modifyForceByDistance)
            {
                if (forceByDistanceModifier > 0f)
                    baseForce *= forceByDistanceModifier;
                else
                    return;
            }
            body.AddForce(baseForce, ForceMode2D);

            float _upliftModifier = 0f;
            if (upliftModifier != 0f)
            {
                _upliftModifier = 1.0f - upliftModifier / radius;
                body.AddForce(Vector2.up * force * _upliftModifier, ForceMode2D);
            }

            if (lookAtMovingDirection)
            {
                Vector2 baseAddingVelocity;
                Vector2 nextStepVelocity = body.velocity;
                Vector3 lookAtPosition = body.transform.position;

                float basePredictedVelocity;
                if (modifyForceByDistance)
                    basePredictedVelocity = (force * forceByDistanceModifier / body.mass) * Time.fixedDeltaTime;
                else
                    basePredictedVelocity = (force / body.mass) * Time.fixedDeltaTime;
                baseAddingVelocity = dir.normalized * basePredictedVelocity;
                nextStepVelocity += baseAddingVelocity;
                if (upliftModifier != 0f)
                {
                    float upliftPredictedVelocity = (force * _upliftModifier / body.mass) * Time.fixedDeltaTime;
                    Vector2 upliftAddingVelocity = Vector2.up * upliftPredictedVelocity;
                    nextStepVelocity += upliftAddingVelocity;
                }
                lookAtPosition += (Vector3)(nextStepVelocity.normalized);
                body.AddLookAt2D(lookAtPosition, lookAtAngleModifier);
            }
        }


        /// <summary>
        /// Applies a force to a Rigidbody2D with uplift modifier that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the explosion.</param>
        /// <param name="explosionPosition">The center of the circle witch the explosion has its effect.</param>
        /// <param name="radius">Radius of the circle within witch the explosion has its effect.</param>
        /// <param name="upliftModifier">Adjustment to the apparent position of the explosion to make it seem to lift objects.</param>
        /// <param name="modifyForceByDistance">If set to <c>true</c> force will be modified by distance from explosion center. \n Note that bodies with center of mass outside of the radius will not be affected with the explosion.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddUpliftedExplosionForce2D(this Rigidbody2D body, float force, Vector3 explosionPosition, float radius, float upliftModifier = 1f, bool modifyForceByDistance = true, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - explosionPosition);
            Vector2 baseForce = dir.normalized * force;
            if (modifyForceByDistance)
            {
                float forceByDistanceModifier = 1.0f - (dir.magnitude / radius);
                if (forceByDistanceModifier > 0f)
                    baseForce *= forceByDistanceModifier;
                else
                    return;
            }
            body.AddForce(baseForce, ForceMode2D);

            float _upliftModifier = 0f;
            if (upliftModifier != 0f)
            {
                _upliftModifier = 1.0f - upliftModifier / radius;
                body.AddForce(Vector2.up * force * _upliftModifier, ForceMode2D);
            }
        }

        #endregion

        #region AddRandomizedExplosion

        /// <summary>
        /// Applies a randomized force to a Rigidbody2D that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the explosion.</param>
        /// <param name="explosionPosition">The center of the circle witch the explosion has its effect.</param>
        /// <param name="radius">Radius of the circle within witch the explosion has its effect.</param>
        /// <param name="modifyForceByDistance">If set to <c>true</c> force will be modified by distance from explosion center. \n Note that bodies with center of mass outside of the radius will not be affected with the explosion.</param>
        /// <param name="randomizeDirection">If set to <c>true</c> Force direction will be multiplied randomly with -1 or 1.</param>
        /// <param name="randomizeForce">If set to <c>true</c> Force will be multiplied with random value between 0 and 1.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddRandomizedExplosionForce2D(this Rigidbody2D body, float force, Vector3 explosionPosition, float radius, bool modifyForceByDistance = true, bool randomizeDirection = true, bool randomizeForce = false, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - explosionPosition);
            Vector2 finalForce = dir.normalized * force;
            if (modifyForceByDistance)
            {
                float forceByDistanceModifier = 1.0f - (dir.magnitude / radius);
                if (forceByDistanceModifier > 0f)
                    finalForce *= forceByDistanceModifier;
                else
                    return;
            }
            if (randomizeDirection)
            {
                finalForce.x *= GetRandomSign();
                finalForce.y *= GetRandomSign();
            }
            if (randomizeForce)
            {
                finalForce.x *= Random.value;
                finalForce.y *= Random.value;
            }
            body.AddForce(finalForce, ForceMode2D);
        }


        /// <summary>
        /// Applies a randomized force to a Rigidbody2D that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the explosion.</param>
        /// <param name="explosionPosition">The center of the circle witch the explosion has its effect.</param>
        /// <param name="randomizeDirection">If set to <c>true</c> Force direction will be multiplied randomly with -1 or 1.</param>
        /// <param name="randomizeForce">If set to <c>true</c> Force will be multiplied with random value between 0 and 1.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddRandomizedExplosionForce2D(this Rigidbody2D body, float force, Vector3 explosionPosition, bool randomizeDirection = true, bool randomizeForce = false, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - explosionPosition);
            Vector2 finalForce = dir.normalized * force;
            if (randomizeDirection)
            {
                finalForce.x *= GetRandomSign();
                finalForce.y *= GetRandomSign();
            }
            if (randomizeForce)
            {
                finalForce.x *= Random.value;
                finalForce.y *= Random.value;
            }
            body.AddForce(finalForce, ForceMode2D);
        }

        #endregion

        #region AttractionForce

        /// <summary>
        /// Applies a attractive force to a Rigidbody2D that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the attraction.</param>
        /// <param name="attractionPosition">The center of the circle witch the attraction has its effect.</param>
        /// <param name="radius">Radius of the circle within witch the attraction has its effect.</param>
        /// <param name="modifyForceByDistance">If set to <c>true</c> force will be modified by distance from explosion center. \n Note that bodies with center of mass outside of the radius will not be affected with the attraction.</param>
        /// <param name="lookAtMovingDirection">If set to <c>true</c> bodies will be instantly rotated towards their moving direction.</param>
        /// <param name="lookAtAngleModifier">Modifies the angle of rotation.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddAttractionForce2D(this Rigidbody2D body, float force, Vector3 attractionPosition, float radius, bool modifyForceByDistance = true, bool lookAtMovingDirection = false, float lookAtAngleModifier = 0f, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - attractionPosition);
            float forceByDistanceModifier = 1.0f - (dir.magnitude / radius);
            Vector2 finalForce = dir.normalized * force;
            if (modifyForceByDistance)
            {
                if (forceByDistanceModifier > 0f)
                    finalForce *= forceByDistanceModifier;
                else
                    return;
            }
            if (force > 0f)
                finalForce *= -1.0f;
            body.AddForce(finalForce, ForceMode2D);

            if (lookAtMovingDirection)
            {
                float predictedVelocity;
                if (modifyForceByDistance)
                    predictedVelocity = (force * forceByDistanceModifier / body.mass) * Time.fixedDeltaTime;
                else
                    predictedVelocity = (force / body.mass) * Time.fixedDeltaTime;
                Vector2 addingVelocity = dir.normalized * predictedVelocity;
                Vector2 nextStepVelocity = (addingVelocity + body.velocity);
                Vector3 lookAtPosition = body.transform.position + (Vector3)(nextStepVelocity.normalized);
                body.AddLookAt2D(lookAtPosition, lookAtAngleModifier);
            }
        }

        /// <summary>
        /// Applies a attractive force to a Rigidbody2D that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the attraction.</param>
        /// <param name="attractionPosition">The center of the circle witch the attraction has its effect.</param>
        /// <param name="radius">Radius of the circle within witch the attraction has its effect.</param>
        /// <param name="modifyForceByDistance">If set to <c>true</c> force will be modified by distance from explosion center. \n Note that bodies with center of mass outside of the radius will not be affected with the attraction.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddAttractionForce2D(this Rigidbody2D body, float force, Vector3 attractionPosition, float radius, bool modifyForceByDistance = true, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - attractionPosition);
            Vector2 finalForce = dir.normalized * force;
            if (modifyForceByDistance)
            {
                float forceByDistanceModifier = 1.0f - (dir.magnitude / radius);
                if (forceByDistanceModifier > 0f)
                    finalForce *= forceByDistanceModifier;
                else
                    return;
            }
            if (force > 0f)
                finalForce *= -1.0f;
            body.AddForce(finalForce, ForceMode2D);
        }

        /// <summary>
        /// Applies a attractive force to a Rigidbody2D that simulates explosion effects.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="force">The force of the attraction.</param>
        /// <param name="attractionPosition">The center of the circle witch the attraction has its effect.</param>
        /// <param name="ForceMode2D">The method used to apply the force to its targets.</param>
        public static void AddAttractionForce2D(this Rigidbody2D body, float force, Vector3 attractionPosition, ForceMode2D ForceMode2D = ForceMode2D.Force)
        {
            Vector2 dir = (body.transform.position - attractionPosition);
            Vector2 finalForce = dir.normalized * force;
            if (force > 0f)
                finalForce *= -1.0f;
            body.AddForce(finalForce, ForceMode2D);
        }

        #endregion



        /// <summary>
        /// Finds the closest position from 'Explosion Position' on RigidBody2D.
        /// </summary>
        /// <param name="body">Rigidbody2D</param>
        /// <param name="explosionPosition">Explosion Position</param>
        /// <returns>The closest position from 'Explosion Position' on RigidBody2D.</returns>
        public static Vector2 GetClosestPosition(Rigidbody2D body, Vector3 explosionPosition)
        {
            Collider2D[] colliders = new Collider2D[body.attachedColliderCount];
            var numOfColliders = body.GetAttachedColliders(colliders);
            float distance = Mathf.Infinity;
            float tmpDistance;
            Vector2 closestPoint, tmpPoint;
            closestPoint = colliders[0].bounds.ClosestPoint(explosionPosition);
            for (int i = 0; i < numOfColliders; i++)
            {
                tmpPoint = colliders[i].bounds.ClosestPoint(explosionPosition);
                tmpDistance = Vector2.Distance(explosionPosition, tmpPoint);
                if (tmpDistance < distance)
                {
                    closestPoint = tmpPoint;
                    distance = tmpDistance;
                }
            }
            return closestPoint;
            // Apply the force at the closest position to "explosionPosition". 
            // This gives more realistic results since the objects start to rotate and it is also more in line with the official AddExplosionForce from Unity.
            // This suggestion and improvement was realized thanks to:   KAMGAM e.U.   https://assetstore.unity.com/publishers/37829
        }
        
        /// <summary>
        /// Rotates the Rigidbody2D towards position.
        /// </summary>
        /// <param name="body">Rigidbody2D.</param>
        /// <param name="position">Position to point towards.</param>
        /// <param name="angleModifier">Modifies the angle of rotation.</param>
        public static void AddLookAt2D(this Rigidbody2D body, Vector3 position, float angleModifier = 0f)
        {
            Vector3 vectorToTarget = position - body.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            body.MoveRotation(angle + angleModifier);
        }

        /// <summary>
        /// Gets the random sign(-1 or 1).
        /// </summary>
        /// <returns>The random sign (-1 or 1).</returns>
        public static float GetRandomSign()
        {
            if (Random.value < 0.5f)
                return 1f;
            else
                return -1f;
        }
        
        //public static float CalculateAngle(Vector3 from, Vector3 to) {
        //	return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
        //}
    }
}
















