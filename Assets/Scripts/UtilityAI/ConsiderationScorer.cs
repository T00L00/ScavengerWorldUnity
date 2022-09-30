using UnityEngine;

namespace ScavengerWorld.AI.UtilityAI
{
    public abstract class ConsiderationScorer : ScriptableObject
    {
        [TextArea(1, 5)] public string Description;
        public abstract float ScoreConsideration(Unit unit, Interactable actionTarget, AnimationCurve responseCurve);
    }
}