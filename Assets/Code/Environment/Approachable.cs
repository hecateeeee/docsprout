﻿namespace Code.Environment
{
    public interface IApproachable
    {
        void OnApproach ();
        void OnDepart ();
        void Interact ();
        void SecondaryInteract ();
    }
}