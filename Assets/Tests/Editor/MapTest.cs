using UnityEngine;

using System;
using System.Collections;

using NUnit.Framework;

namespace Testing
{
    [TestFixture]
    class Test
    {
        private static readonly int[] DATA =
        {
            0, 0, 0,
            0, 0, 1,
            0, 0, 0,
            1, 1, 1,
        };
        
        private const int MAP_ROWS = 4;
        private const int MAP_COLS = 3;
        private const float CW = Constants.CELL_WIDTH;
        private const float CH = Constants.CELL_HEIGHT;

        [Test]
        public void TestGetCell()
        {
        }

        private MockCollider CreateCollider(float cx, float cy)
        {
            return new MockCollider(cx * CW, cy * CH, CW, CH, delegate(Cell cell)
            {
                return false;
            });
        }
    }

    class MapMock : Map
    {
        private int[] data;
        private int rows;
        private int cols;

        public MapMock(int[] data, int rows, int cols)
        {
            this.data = data;
            this.rows = rows;
            this.cols = cols;

            OnStart();
        }
        
        protected override void CreateCells()
        {
            CreateCells(data, rows, cols);
        }
    }

    delegate bool MockColliderCallback(Cell cell);

    class MockCollider : IMapCollider
    {
        public float m_X;
        public float m_Y;
        public float m_Width;
        public float m_Height;
        public MockColliderCallback m_CollisionCallback;

        public MockCollider(float x, float y, float width, float height, MockColliderCallback collisionCallback)
        {
            m_X = x;
            m_Y = y;
            m_Width = width;
            m_Height = height;
            m_CollisionCallback = collisionCallback;
        }

        public bool OnCollision(Cell cell)
        {
            return m_CollisionCallback(cell);
        }

        public Rect colliderRect
        {
            get { return new Rect(m_X, m_Y, m_Width, m_Height); }
        }
    }
}
