using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BaseProtection.Units
{
    public class UnitFactory
    {
        private UnitConfig _unit;
        private List<UnitStats> _stats;
        private Transform _parent;
        private HashAnimation _animations;
        private Action _onUnitDied;

        public UnitFactory(UnitConfig unit, UnitStats stats, Transform parent, Action onUnitDied = null)
        {
            _unit = unit;
            _parent = parent;
            _onUnitDied = onUnitDied;

            UnitStats unitStats = new UnitStats(stats);
            
            _stats = new List<UnitStats>();
            _stats.Add(unitStats);
            
            _animations = new HashAnimation();
        }

        public Unit CreateUnit()
        {
            Unit instance = GameObject.Instantiate(_unit.Prefab, _parent);
            instance.Init(_unit, _stats.Last(), _animations, _onUnitDied);
            
            return instance;
        }

        public void UpdateUnitStats(WaveUnit config)
        {
            _unit = config.Unit;
            
            UnitStats unitStats = new UnitStats(config.Stats);
            _stats.Add(unitStats);
        }
    }
}