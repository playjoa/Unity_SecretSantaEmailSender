using System;

namespace AppWideSystems.SecretSantaSystem.Data
{
    [Serializable]
    public struct SecretSantaResult
    {
        public SecretSantaUserData User;
        public SecretSantaUserData DrawnedUser;
        public bool NotifiedUser;

        public SecretSantaResult(SecretSantaUserData user, SecretSantaUserData drawnUser)
        {
            User = user;
            DrawnedUser = drawnUser;
            NotifiedUser = false;
        }

        public void SetNotifiedUser()
        {
            NotifiedUser = true;
        }
        
        public void ResetNotifiedUser()
        {
            NotifiedUser = false;
        }
    }
}