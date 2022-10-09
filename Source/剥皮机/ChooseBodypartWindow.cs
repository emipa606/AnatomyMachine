using System.Linq;
using UnityEngine;
using Verse;

namespace LingMod;

public class ChooseBodypartWindow : Window
{
    private static string instructionString = "";
    private readonly Building_Dismemberment thing;
    private Vector2 resultsAreaScroll;

    public ChooseBodypartWindow(Building_Dismemberment thing)
    {
        optionalTitle = "ChooseBodypartWindowMain".Translate();
        preventCameraMotion = false;
        absorbInputAroundWindow = false;
        draggable = true;
        doCloseX = true;
        instructionString = "ChooseBodypartWindowDesc".Translate();
        this.thing = thing;
        var bodyParts = (from x in thing.bodyParts
            where !x.IsCorePart
            select x).ToList();
        thing.pairsAs.Clear();
        foreach (var item in bodyParts)
        {
            thing.pairsAs.Add(new KeyValuePairsA(item, false));
        }
    }

    public override Vector2 InitialSize => new Vector2(640f, 480f);

    public override void DoWindowContents(Rect inRect)
    {
        //throw new NotImplementedException();
        Text.Font = GameFont.Small;
        var rect = new Rect(inRect);
        rect.height = Text.CalcHeight(instructionString, rect.width) + 2f;
        Widgets.Label(rect, instructionString);
        var rect2 = new Rect(rect);
        rect2.y += rect.height + 2f;
        rect2.height = inRect.height - rect2.y - 2f;
        var rect3 = new Rect(rect2);
        rect3.width -= 16f;
        var num = (Text.LineHeight * 2.25f) + 2f;
        var num2 = thing.pairsAs.Count > 0 ? (thing.pairsAs.Count + 1) * num : 0f;
        rect3.height = num2;
        Widgets.BeginScrollView(rect2, ref resultsAreaScroll, rect3);
        var rect4 = new Rect(rect3) { height = num };

        if (thing.pairsAs.Count > 0)
        {
            foreach (var BodyPart in thing.pairsAs)
            {
                Widgets.DrawHighlightIfMouseover(rect4);
                var rect6 = new Rect(rect4);
                rect6.width = rect6.height;
                var rectTemp = new Rect(rect6);
                rectTemp.width *= 0.8f;
                rectTemp.height = rectTemp.width;
                Widgets.ThingIcon(rectTemp, thing.Pan);
                var rect7 = new Rect(rect4);
                rect7.width = rect7.width - rect6.width - (num * 2f);
                rect7.x += rect6.width;
                var strr = string.IsNullOrEmpty(BodyPart.Key.customLabel)
                    ? BodyPart.Key.LabelCap
                    : BodyPart.Key.customLabel;

                if (BodyPart.Key.def.spawnThingOnRemoved != null)
                {
                    strr = strr + " $" + BodyPart.Key.def.spawnThingOnRemoved.BaseMarketValue;
                }

                Widgets.Label(rect7, strr);
                var rect8 = new Rect(rect4);
                rect8.width = rect8.height;
                rect8.x = rect7.x + rect7.width;
                TooltipHandler.TipRegion(rect8, "ChooseBodypartWindowSel".Translate());
                if (BodyPart.Value)
                {
                    if (Widgets.ButtonImage(rect8.ContractedBy(rect8.width / 4f), DismembermentTieTu.DuiDui))
                    {
                        //BodyPart.Value =!BodyPart.Value;
                        BodyPart.SetValue(false);
                        //this.Close();
                    }
                }
                else
                {
                    if (Widgets.ButtonImage(rect8.ContractedBy(rect8.width / 4f), DismembermentTieTu.BuBuDui))
                    {
                        //BodyPart.Value =!BodyPart.Value;
                        BodyPart.SetValue(true);
                        //this.Close();
                    }
                }

                var rect9 = new Rect(rect4);
                rect9.width = rect9.height;
                rect9.x = rect8.x + rect8.width;
                if (BodyPart.Key.def.spawnThingOnRemoved != null)
                {
                    Widgets.InfoCardButton(rect9.x + (rect9.width / 2f) - 12f, rect9.y + (rect9.height / 2f) - 12f,
                        BodyPart.Key.def.spawnThingOnRemoved);
                    TooltipHandler.TipRegion(rect4, BodyPart.Key.def.spawnThingOnRemoved.DescriptionDetailed);
                }


                rect4.y += num;
            }
        }

        Text.Anchor = TextAnchor.UpperLeft;
        Widgets.EndScrollView();
    }
}