﻿using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        public GameObject HPBar;
        public GameObject inventory;
        public Sprite emptySlot;
        public GameObject targets;
        public GameObject target_prefab;
        public GameObject weapon;
        public GameObject unitHpBar;
        public GameObject history;

        private GameObject player;
        private List<Image> slots;
        //private ArrayList hpBars;
        // Use this for initialization
        void Start()
        {
           // hpBars = new ArrayList();
            var _player = GameObject.FindWithTag("Player");
            if (_player != null)
            {
                player = _player;
                player.GetComponent<Health>().PropertyChanged += InstanceOnPropertyChanged;
                player.GetComponent<Inventory>().PropertyChanged += InstanceOnPropertyChanged;
                player.GetComponent<Targets>().PropertyChanged += InstanceOnPropertyChanged;
                player.GetComponent<Stories>().PropertyChanged += InstanceOnPropertyChanged;
                player.GetComponent<PlayerController>().PropertyChanged += InstanceOnPropertyChanged;
                print("find player!");
            }
            slots = new List<Image>();
            for (int i = 0; i < 3; i++)
            {
                slots.Add(inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>());
            }
            UpdateView();
        }

        private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            if (player != null)
            {
                setHP(player.GetComponent<Health>().GetHealth(), player.GetComponent<Health>().GetMaxHealth());
                updateInventory(player.GetComponent<Inventory>().GetItems());
                updateTargets(player.GetComponent<Targets>().GetTargets());
                updateStories(player.GetComponent<Stories>().GetStories());
                var _controller = player.GetComponent<PlayerController>();
                updateWeapon(_controller.currentFirearm, _controller.currentFirearmIndex);
            }
        }

        private void setHP(float _val, float _max_val)
        {
            HPBar.transform.GetChild(0).GetComponent<Image>().fillAmount = _val / _max_val;
            HPBar.GetComponentInChildren<Text>().text = _val.ToString() + "/" + _max_val.ToString();
        }
        private void updateInventory(List<Collectible> _list)
        {
            int count = 0;
            foreach (Image img in slots)
            {
                img.sprite = emptySlot;
                img.color = Color.white;
            }
            foreach (Collectible item in _list)
            {
                var _sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
                var _color = item.gameObject.GetComponent<SpriteRenderer>().color;
                
                if (_sprite != null)
                {
                    slots[count].sprite = _sprite;
                    if (_color != null)
                    {
                        slots[count].color = _color;
                    }
                    //slots[count].transform.GetComponentInParent<Animation>().Play();
                    count++;
                }

                

                if (count > 3) break;
            }
        }

        private void updateTargets(List<GameEvents> _list)
        {
            targets.transform.GetChild(0).gameObject.GetComponent<Text>().enabled = true;
            for (int i = 1; i < targets.transform.childCount; i++)
            {
                Destroy(targets.transform.GetChild(i).gameObject);
            }
            foreach (GameEvents target in _list)
            {
                if(!target.end)
                {
                    var _target = Instantiate(target_prefab, targets.transform.position, Quaternion.identity);
                    _target.transform.SetParent(targets.transform);
                    _target.transform.GetChild(0).GetComponent<Text>().text = target.text;
                }
            }
            if(targets.transform.childCount == 1)
            {
                targets.transform.GetChild(0).gameObject.GetComponent<Text>().enabled = false;
            }
        }

        private void updateStories(List<StoryPart> _list)
        {
            var _history = history.GetComponent<HistoryViewer>();

            if (!_history) return;

            foreach (StoryPart story in _list)
            {
                if (!_history.bStoryOpen[story.index])
                {
                    _history.unlockStory(story.index);
                }
            }
        }

        private void updateWeapon(Firearm _weapon, int _index)
        {
            if(_index == 0)
            {
                weapon.transform.GetChild(0).GetComponent<Text>().text = "--";
            }
            else
            {
                var _text = weapon.transform.GetChild(0).GetComponent<Text>().text = _weapon.ammo.ToString();
            }

            weapon.GetComponent<Image>().sprite = _weapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            weapon.GetComponent<Image>().color = _weapon.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        }
        public void createHPBar(Transform _transform)
        {
            if(unitHpBar != null)
            {
                var _obj = Instantiate(unitHpBar, transform);
                _obj.GetComponent<hpBarController>().followBy = _transform;
            }
        }

    }   
}
