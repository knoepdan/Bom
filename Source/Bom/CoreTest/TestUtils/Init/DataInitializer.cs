using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Core.Data;
using Core.Model;
using Core.DataAccess;

namespace Core.TestUtils.Init
{
    public class DataInitializer
    {
        private readonly ModelContext _context;

        private readonly PathNodeProvider _pathNodeProvider;

        public DataInitializer(ModelContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            _context = context;
            _pathNodeProvider = new PathNodeProvider(this._context);
        }


        public void AddTestData()
        {
            CheckAndThrowIfAlreadyTestdata();

            AddAnimalNodes();
            AddPlantNodes();
        }
        
        private void AddPlantNodes()
        {
            var plants = AddNode("Plants", null);
            var trees = AddNode("Trees", plants);
            var flowers = AddNode("Flowers", plants);
            var gras = AddNode("Gras", plants);
        }


        private void AddAnimalNodes()
        {
            var animals = AddNode("Animals", null);
            var mammals = AddNode("Mammals", animals);
            var birds = AddNode("Birds", animals);
            var reptiles = AddNode("Reptiles", animals);

            // birds
            AddNode("Songbirds", birds);
            AddNode("Non-Songbirds", birds);

            // reptiles
            AddNode("Snakes", reptiles);
            AddNode("Turtles", reptiles);
            AddNode("Crocodiles", reptiles);
            AddNode("Lizards", reptiles);

            // mammals
            var x = AddNode("Xenarthra", mammals);
            var pil = AddNode("Pilosa", x);
            var ant = AddNode("Anteater", pil);
            // anteaters
            var cycl = AddNode("Cyclopedidae", ant);
            AddNode("Silky Anteater", cycl);
            AddNode("Tamandua", cycl);
            var myrmecophagidae = AddNode("Myrmecophagidae", ant);
            AddNode("Giant Anteater", myrmecophagidae);
            // sloths
            var threeToedSloths = AddNode("Three-toed sloths", pil);
            AddNode("Maned three-toed sloth", threeToedSloths);
            AddNode("Brown-throated three-toed sloth", threeToedSloths);

            var twoToedSloths = AddNode("Twoe-toed sloths", pil);
            AddNode("Linnaeus's two-toed sloth", twoToedSloths);
            AddNode("Hoffmann's two-toed sloth", twoToedSloths);
        }


        private Path AddNode(string nodeTitle, Path parentPath, bool saveOnAdd = true)
        {
            var path = this._pathNodeProvider.AddNodeWithPath(parentPath, nodeTitle);
            if (saveOnAdd)
            {
                this._pathNodeProvider.ModelContext.SaveChanges();
            }

            return path;
        }


        private void CheckAndThrowIfAlreadyTestdata()
        {
            var tmpNode = _context.GetNodes().FirstOrDefault();
            if (tmpNode != null)
            {
                throw new Exception($"There is already test data {tmpNode.Title}");
            }
        }

    }

}
