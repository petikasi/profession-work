using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.PrefabsAndAbstracts
{
    public abstract class DeckListerAbstract<T>:MonoBehaviour
    {

        [SerializeField] protected GameObject itemPrefab;
        [SerializeField] protected Transform gridParent;
        [SerializeField] protected Button nextButton;
        [SerializeField] protected Button previousButton;

        public int itemsPerPage = 9;
        protected int currentPage = 0;

        protected virtual void OnEnable()
        {
            nextButton.onClick.AddListener(NextPage);
            previousButton.onClick.AddListener(PreviousPage);
            if (gridParent.childCount != GetItems().Count)
            {
                CreatePage();
            }
            ShowPage();



        }

        protected virtual void OnDisable()
        {
            nextButton.onClick.RemoveListener(NextPage);
            previousButton.onClick.RemoveListener(PreviousPage);

            ClearPage();
        }

        protected abstract List<T> GetItems();
        protected abstract void SetupItem(GameObject obj, T item);

        public void CreatePage()
        {
            DestroyPage();

            var items = GetItems();

            int startIndex = currentPage * itemsPerPage;
            int endIndex = Mathf.Min(startIndex + itemsPerPage, items.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                GameObject obj = Instantiate(itemPrefab, gridParent);
                SetupItem(obj, items[i]);
            }

            previousButton.gameObject.SetActive(currentPage > 0);
            nextButton.gameObject.SetActive(endIndex < items.Count);
        }
        public void DestroyPage()
        {
            foreach (Transform child in gridParent)
            {
                Destroy(child.gameObject);
            }
        }

        public void ClearPage()
        {
            foreach (Transform child in gridParent)
            {
                child.gameObject.SetActive(false);
            }
        }
        public void ShowPage()
        {
            foreach (Transform child in gridParent)
            {
                child.gameObject.SetActive(true);
            }
        }

        private void NextPage()
        {
            currentPage++;
            CreatePage();
        }

        private void PreviousPage()
        {
            currentPage--;
            CreatePage();
        }



    }
}
