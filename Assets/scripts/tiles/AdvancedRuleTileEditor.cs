#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(AdvancedRuleTile))]
    [CanEditMultipleObjects]

    // AdvancedRuleTileEditor adds pictures to new rules just as in the old ones.
    public class AdvancedRuleTileEditor : RuleTileEditor
    {
        public Texture2D AnyIcon;
        public Texture2D SpecifiedIcon;
        public Texture2D NothingIcon;

        // Is called when the picture for the rule is supposed to be drawn.
        public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
        {
            switch (neighbor)
            {
                case AdvancedRuleTile.Neighbor.Any:
                    GUI.DrawTexture(rect, AnyIcon);
                    return;
                case AdvancedRuleTile.Neighbor.Specified:
                    GUI.DrawTexture(rect, SpecifiedIcon);
                    return;
                case AdvancedRuleTile.Neighbor.Nothing:
                    GUI.DrawTexture(rect, NothingIcon);
                    return;
            }

            base.RuleOnGUI(rect, position, neighbor);
        }
    }
}
#endif