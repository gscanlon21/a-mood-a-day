﻿using Core.Attributes;
using Core.Models.Components.SubComponents;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Components;

/// <summary>
/// Controls access to user features.
/// </summary>
[Flags]
public enum Component
{
    None = 0,

    [Display(Name = "Mood Tracking")]
    Mood = 1 << 0, // 1

    [Display(Name = "Sleep Tracking")]
    Sleep = 1 << 1, // 2

    [Display(Name = "People Tracking")]
    People = 1 << 2, // 4

    [Display(Name = "Symptom Tracking")]
    Symptom = 1 << 3, // 8

    [Display(Name = "Emotion Tracking")]
    Emotion = 1 << 4, // 16

    [Display(Name = "Activity Tracking")]
    Activity = 1 << 5, // 32

    [Display(Name = "Medicine Tracking")]
    Medicine = 1 << 6, // 64

    [Display(Name = "Acute Stress Severity Tracking")]
    AcuteStressSeverity = 1 << 7, // 128

    [Display(Name = "Agoraphobia Severity Tracking")]
    AgoraphobiaSeverity = 1 << 8, // 256

    [Display(Name = "Depression  Severity Tracking")]
    DepressionSeverity = 1 << 9, // 512

    [Display(Name = "Dissociative Severity Tracking")]
    DissociativeSeverity = 1 << 10, // 1024

    [Display(Name = "Generalized Anxiety Severity")]
    GeneralizedAnxietySeverity = 1 << 11, // 2048

    [Display(Name = "Panic Disorder Severity")]
    PanicSeverity = 1 << 12, // 4096

    [Display(Name = "PTSD Severity")]
    PTSDSeverity = 1 << 13, // 8192

    [Display(Name = "Social Anxiety Severity")]
    SocialAnxietySeverity = 1 << 14, // 16384

    [Display(Name = "Journal")]
    Journal = 1 << 15, // 32768

    [Display(Name = "Complete Metabolic Panel"), SubComponent<CompleteMetabolicPanel>()]
    CompleteMetabolicPanel = 1 << 16, // 65536

    [Display(Name = "Tasks")]
    Tasks = 1 << 17, // 131072

    [Display(Name = "Dry Eyes")]
    DryEyes = 1 << 18, // 262144

    [Display(Name = "Feast Allergens")]
    FeastAllergens = 1 << 19, // 524288

    [Display(Name = "Cbc W Auto Differential"), SubComponent<CbcWAutoDiff>()]
    CbcWAutoDiff = 1 << 20, // 1048576

    [Display(Name = "Blood Work"), SubComponent<BloodWork>()]
    BloodWork = 1 << 21, // 2097152

    [Display(Name = "Depression")]
    Depression = 1 << 22, // 4194304

    All = Mood | Sleep | People | Symptom | Emotion | Activity | Medicine
        | AcuteStressSeverity | AgoraphobiaSeverity | DepressionSeverity | DissociativeSeverity
        | GeneralizedAnxietySeverity | PanicSeverity | PTSDSeverity | SocialAnxietySeverity
        | Journal | CompleteMetabolicPanel | Tasks | DryEyes | FeastAllergens | CbcWAutoDiff
        | BloodWork | Depression
}
