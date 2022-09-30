using UnityEngine;

namespace ScavengerWorld.AI.UtilityAI
{
    [System.Serializable]
    public class Consideration
    {
        public string Name;
        public float Weight = 1f;
        public AnimationCurve responseCurve;
        public ConsiderationScorer scorer;

        public Interactable Target { get; set; }

        private float score;
        public float Score
        {
            get { return score; }
            set
            {
                this.score = Mathf.Clamp01(value);
            }
        }

        public Consideration(string name, float weight, AnimationCurve responseCurve, ConsiderationScorer scorer)
        {
            Name = name;
            Weight = weight;
            this.responseCurve = responseCurve;
            this.scorer = scorer;
        }

        public void ScoreConsideration(Unit unit, Interactable target, int considerationCount)
        {
            float rawScore = scorer.ScoreConsideration(unit, target, responseCurve);
            Score = CompensateScore(rawScore, considerationCount);
        }

        public float CompensateScore(float score, int considerationsCount)
        {
            float originalScore = score;
            float modificationFactor = 1f - (1f / considerationsCount);
            float makeupValue = (1 - originalScore) * modificationFactor;
            score = originalScore + (makeupValue * originalScore);
            return score;
        }

        public SerializedConsideration Serialize()
        {
            return new SerializedConsideration(this);
        }
    }
}