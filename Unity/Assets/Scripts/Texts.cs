using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Texts
{
    public enum Language
    {
        en, fr
    }
    
    public static Language language { get; set; }

    public static string getSelectAModelToSpawn()
    {
        switch (language)
        {
            case Language.fr:
                return "Explorez ou placez un tag !";
            case Language.en:
            default :
                return "Explore or place a tag !";
        }
    }

    public static string getTakeScreenshot()
    {
        switch (language)
        {
            case Language.fr:
                return "Prendre la photo";
            case Language.en:
            default :
                return "Take a picture";
        }
    }

    public static string getRefreshPage()
    {
        switch (language)
        {
            case Language.fr:
                return "Actualiser la page";
            case Language.en:
            default :
                return "Refresh page";
        }
    }

    public static string getSkip()
    {
        switch (language)
        {
            case Language.fr:
                return "Ignorer";
            case Language.en:
            default :
                return "Skip";
        }
    }
    
    public static string getScreenshotTaken()
    {
        switch (language)
        {
            case Language.fr:
                return "Capture d'écran effectuée";
            case Language.en:
            default :
                return "Screenshot taken";
        }
    }

    public static string getScreenshotSkipped()
    {
        switch (language)
        {
            case Language.fr:
                return "Capture d'écran ignorée";
            case Language.en:
            default :
                return "Screenshot skipped";
        }
    }

    public static string getSelectAnotherModel()
    {
        switch (language)
        {
            case Language.fr:
                return "Selectionnez un autre modèle à placer";
            case Language.en:
            default :
                return "Select another model to place";
        }
    }

    public static string getSavingSpawnedObject()
    {
        switch (language)
        {
            case Language.fr:
                return "Savegarde de l'objet placé";
            case Language.en:
            default :
                return "Saving spawned object";
        }
    }

    public static string getMoveYourDevice()
    {
        switch (language)
        {
            case Language.fr:
                return "Déplacer votre appareil pour capturer plus de données d'environment";
            case Language.en:
            default :
                return "Move your device to capture more environment data";
        }
    }

    public static string getSaving()
    {
        switch (language)
        {
            case Language.fr:
                return "Sauvegarde...";
            case Language.en:
            default :
                return "Saving...";
        }
    }

    public static string getSavedAnchor()
    {
        switch (language)
        {
            case Language.fr:
                return "Ancre sauvegardée";
            case Language.en:
            default :
                return "Saved anchor succesfully";
        }
    }

    public static string getTakeAScreenshot()
    {
        switch (language)
        {
            case Language.fr:
                return "Prenez une photo !";
            case Language.en:
            default :
                return "Take a picture !";
        }
    }

    public static string getErrorResponse()
    {
        switch (language)
        {
            case Language.fr:
                return "Réponse d'erreur";
            case Language.en:
            default :
                return "Error response";
        }
    }

    public static string getFailedToSave()
    {
        switch (language)
        {
            case Language.fr:
                return "Echec de la sauvegarde";
            case Language.en:
            default :
                return "Failed to save";
        }
    }

    public static string getGuestMode()
    {
        switch (language)
        {
            case Language.fr:
                return "Explorez pour trouver des tags ou basculez pour créer un compte";
            case Language.en:
            default :
                return "Explore to find tags or switch to create an account";
        }
    }

    public static string getClickSave()
    {
        switch (language)
        {
            case Language.fr:
                return "Cliquez sur sauvegarder";
            case Language.en:
            default :
                return "Click save !";
        }
    }

    public static string getSave()
    {
        switch (language)
        {
            case Language.fr:
                return "Sauvegarder";
            case Language.en:
            default :
                return "Save";
        }
    }

    public static string getTouchASurface()
    {
        switch (language)
        {
            case Language.fr:
                return "Touchez une surface pour placer le tag";
            case Language.en:
            default :
                return "Touch a surface to place the tag";
        }
    }

    public static string getAnchorClicked()
    {
        switch (language)
        {
            case Language.fr:
                return "Ancre touchée";
            case Language.en:
            default :
                return "Anchor clicked";
        }
    }
    
    public static string getPlacedBy()
    {
        switch (language)
        {
            case Language.fr:
                return "Placée par";
            case Language.en:
            default :
                return "Placed by";
        }
    }
    
    public static string getDetails()
    {
        switch (language)
        {
            case Language.fr:
                return "Détails";
            case Language.en:
            default :
                return "Details";
        }
    }
    
    public static string getCancel()
    {
        switch (language)
        {
            case Language.fr:
                return "Annuler";
            case Language.en:
            default :
                return "Cancel";
        }
    }
    
    public static string getPhysicalWorld()
    {
        switch (language)
        {
            case Language.fr:
                return "Monde Physique";
            case Language.en:
            default :
                return "Physical World";
        }
    }
    
    public static string getSwitch()
    {
        switch (language)
        {
            case Language.fr:
                return "Basculer";
            case Language.en:
            default :
                return "Switch";
        }
    }
    
    public static string get()
    {
        switch (language)
        {
            case Language.fr:
                return "";
            case Language.en:
            default :
                return "";
        }
    }



}
