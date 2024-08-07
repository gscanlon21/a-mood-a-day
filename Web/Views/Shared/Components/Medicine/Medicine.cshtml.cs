﻿using Core.Code.Helpers;
using Data.Entities.Footnote;
using Data.Entities.User;
using Web.ViewModels;

namespace Web.Views.Shared.Components.Medicine;


public class MedicineViewModel
{
    public MedicineViewModel(IList<UserMedicine>? userMoods, List<UserCustom> customs)
    {
        Customs = customs;
        //Mood = currentWeight.GetValueOrDefault();
        if (userMoods != null)
        {
            var flatMap = userMoods.SelectMany(m =>
            {
                return m.UserCustoms.Select(c => new UserCustomGroup(m.Date, c.Type, c.Id, c.Name));
            });

            foreach (var custom in Customs)
            {
                // Skip today, start at 1, because we append the current weight onto the end regardless.
                Xys.AddRange(Enumerable.Range(1, 365).Select(i =>
                {
                    var date = DateHelpers.Today.AddDays(-i);
                    return new XCustom(date, flatMap.FirstOrDefault(uw => uw.Date == date && uw.Id == custom.Id), custom);
                }).Where(xy => xy.Y != null).Reverse().Append(new XCustom(DateHelpers.Today, flatMap.FirstOrDefault(um => um.Date == DateHelpers.Today && um.Id == custom.Id), custom)).ToList());
            }
        }
    }

    public string Token { get; init; } = null!;
    public Data.Entities.User.User User { get; init; } = null!;

    public UserMedicine UserMood { get; init; } = null!;
    public UserMedicine? PreviousMood { get; init; }

    internal List<XCustom> Xys { get; init; } = [];
    internal List<IGrouping<UserCustom, XCustom>> XysGrouped => Xys.GroupBy(xy => xy.Label).ToList();

    public List<UserCustom> Customs { get; set; } = [];
}
