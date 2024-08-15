using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;
using CommandTerminal;
using FarlandsCoreMod.Utiles;

namespace FarlandsCoreMod.TerminalAuxiliar
{
    internal class TerminalAux : IManager
    {
        public void Init()
        {
            //crearTerminal();
        }

        // ---------------------------------------- TERMINAL ---------------------------------------- //
        void crearTerminal()
        {
            GameObject _terminal = new GameObject();
            _terminal.AddComponent<CommandTerminal.Terminal>();

            // buscar el GameManager y añadirle como hijo _terminal
            GameObject _gameManager = GameObject.Find("(singleton) FarlandsGameManager");
            if (_gameManager != null)
            {
                _terminal.transform.SetParent(_gameManager.transform);
            }
            else
            {
                Terminal.Log("No se encontró el GameManager");
            }

            Terminal.Log("Terminal Creada 1");
            Console.WriteLine("Terminal Creada 3");
        }





        // --- Comandos Fmods --- //
        [RegisterCommand(Help = "Devuelve el nombre de la escena")]
        static void CommandEscen(CommandArg[] args)
        {
            Scene _escenaActiva = SceneManager.GetActiveScene();
            string _nombreEscena = _escenaActiva.name;
            Terminal.Log("Nombre escena activa: " + _nombreEscena);
        }



        /*
            [RegisterCommand(Help = "Devuelve el nombre de todos los objetos de la escena")]
            static void CommandObj(CommandArg[] args)
            {
                GameObject[] allObjects = FindObjectsOfType<GameObject>();

                foreach (GameObject obj in allObjects)
                {
                    Console.WriteLine("Objeto en la escena: " + obj.name);
                }
            }
        */
    }
}
