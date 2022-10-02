using UnityEngine;

namespace ScavengerWorld.AI.UAI
{
    public abstract class ConsiderationScorer : ScriptableObject
    {
        [TextArea(1, 5)] public string Description;
        public abstract float ScoreConsideration(Unit unit, Interactable actionTarget, AnimationCurve responseCurve);

        public static ConsiderationScorer Load(string name)
        {
            return Resources.Load<ConsiderationScorer>($"ConsiderationScorer/{name}");
        }
    }
}