using SimpleRepositories.Bundles.Tests.GarbageTypes;

namespace SimpleRepositories.Bundles.Tests;

[TestClass]
public class BundleRepositoryTester
{
    [TestClass]
    public class Update : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenEntityIsNull_Throw()
        {
            //Arrange
            Garbage entity = null!;

            //Act
            var action = () => Instance.Update(entity);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entity));
        }

        [TestMethod]
        public void WhenEntityIsNotPartOfRepository_Throw()
        {
            //Arrange
            var entity = Dummy.Build<Garbage>().With(x => x.Id, GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>()).Create();

            //Act
            var action = () => Instance.Update(entity);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Garbage), entity.Id));
        }

        [TestMethod]
        public void WhenEntityAlreadyExistsInRepository_UpdateIt()
        {
            //Arrange
            var entity = new Garbage
            {
                Id = 3,
                Name = "Merry",
                Level = 19
            };

            //Act
            Instance.Update(entity);

            //Assert
            Instance[3].Should().Be(entity);
        }
    }

    [TestClass]
    public class UpdateMany_Params : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.UpdateMany();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToUpdate, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenContainsNullEntities_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.UpdateMany(GarbageBundleRepository.Items[0] with { Name = "Bogus" }, null!, GarbageBundleRepository.Items[1] with { Name = "Not gonna work" });

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TryingToUpdateNulls, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenEntityIsNotPartOfRepository_Throw()
        {
            //Arrange
            var entity = Dummy.Build<Garbage>().With(x => x.Id, GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>()).Create();

            //Act
            var action = () => Instance.UpdateMany(entity);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Garbage), entity.Id));
        }

        [TestMethod]
        public void WhenEntityAlreadyExistsInRepository_UpdateIt()
        {
            //Arrange
            var entity = new Garbage
            {
                Id = 3,
                Name = "Merry",
                Level = 19
            };

            //Act
            Instance.UpdateMany(entity);

            //Assert
            Instance[3].Should().Be(entity);
        }

        [TestMethod]
        public void WhenUpdatingManyExistingEntities_UpdateThem()
        {
            //Arrange
            var entity1 = GarbageBundleRepository.Items[1] with { Name = "Not Jerry" };
            var entity2 = GarbageBundleRepository.Items[2] with { Name = "Not Terry" };

            //Act
            Instance.UpdateMany(entity1, entity2);

            //Assert
            Instance.FetchById(1).Should().Be(entity1);
            Instance.FetchById(3).Should().Be(entity2);
        }
    }

    [TestClass]
    public class UpdateMany_Enumerable : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesNull_Throw()
        {
            //Arrange
            IEnumerable<Garbage> entities = null!;

            //Act
            var action = () => Instance.UpdateMany(entities);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entities));
        }

        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange
            var entities = new List<Garbage>();

            //Act
            var action = () => Instance.UpdateMany(entities);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToUpdate, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenContainsNullEntities_Throw()
        {
            //Arrange
            var entities = new List<Garbage>
            {
                GarbageBundleRepository.Items[0] with {Name = "Bogus"},
                null!,
                GarbageBundleRepository.Items[1] with {Name = "Not gonna work"},
            };

            //Act
            var action = () => Instance.UpdateMany(entities);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TryingToUpdateNulls, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenEntityIsNotPartOfRepository_Throw()
        {
            //Arrange
            var entities = new List<Garbage> { Dummy.Build<Garbage>().With(x => x.Id, GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>()).Create() };

            //Act
            var action = () => Instance.UpdateMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Garbage), entities.Single().Id));
        }

        [TestMethod]
        public void WhenEntityAlreadyExistsInRepository_UpdateIt()
        {
            //Arrange
            var entities = new List<Garbage>
            {
                new Garbage
                {
                    Id = 3,
                    Name = "Merry",
                    Level = 19
                }
            };

            //Act
            Instance.UpdateMany(entities);

            //Assert
            Instance[3].Should().Be(entities.Single());
        }

        [TestMethod]
        public void WhenUpdatingManyExistingEntities_UpdateThem()
        {
            //Arrange
            var entities = new List<Garbage>
            {
                GarbageBundleRepository.Items[1] with { Name = "Not Jerry" },
                GarbageBundleRepository.Items[2] with { Name = "Not Terry" }
            };

            //Act
            Instance.UpdateMany(entities);

            //Assert
            Instance.FetchById(1).Should().Be(entities[0]);
            Instance.FetchById(3).Should().Be(entities[1]);
        }
    }

    [TestClass]
    public class Insert : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenEntityIsNull_Throw()
        {
            //Arrange
            Garbage entity = null!;

            //Act
            var action = () => Instance.Insert(entity);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entity));
        }

        [TestMethod]
        public void WhenIdIsChangedByAbstractCreateMethod_Throw()
        {
            //Arrange
            var bogus = new BogusGarbageBundleRepository();

            //Act
            var action = () => bogus.Insert(Dummy.Create<Garbage>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.IdWasChangedBeforeInsert, nameof(Garbage), 5, 4));
        }

        [TestMethod]
        public void WhenAbstractMethodIsNotBogus_InsertAtEndWithAutoIncrementedId()
        {
            //Arrange
            var entity = Dummy.Create<Garbage>();

            //Act
            Instance.Insert(entity);

            //Assert
            Instance.Contains(x => x == entity with { Id = 5 });
        }
    }

    [TestClass]
    public class InsertMany_Params : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.InsertMany();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToInsert, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenOneEntityIsNull_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.InsertMany(Dummy.Create<Garbage>(), null!, Dummy.Create<Garbage>());

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TryingToInsertNulls, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenIdIsChangedByAbastractCreateMethod_Throw()
        {
            //Arrange
            var bogus = new BogusGarbageBundleRepository();

            //Act
            var action = () => bogus.InsertMany(Dummy.Create<Garbage>(), Dummy.Create<Garbage>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.IdWasChangedBeforeInsert, nameof(Garbage), 5, 4));
        }

        [TestMethod]
        public void WhenNoEntityIsNullAndNoIdIsModifiedByAbstractCreateMethod_AddAllByIncrementingId()
        {
            //Arrange
            var items = Dummy.CreateMany<Garbage>(3).ToArray();

            //Act
            Instance.InsertMany(items);

            //Assert
            Instance.Contains(x => x == items[0] with { Id = 5 });
            Instance.Contains(x => x == items[1] with { Id = 6 });
            Instance.Contains(x => x == items[2] with { Id = 7 });
        }
    }

    [TestClass]
    public class InsertMany_Enumerable : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesIsNull_Throw()
        {
            //Arrange
            IEnumerable<Garbage> entities = null!;

            //Act
            var action = () => Instance.InsertMany(entities);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entities));
        }

        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.InsertMany(new List<Garbage>());

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToInsert, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenOneEntityIsNull_Throw()
        {
            //Arrange
            var entities = new List<Garbage> { Dummy.Create<Garbage>(), null!, Dummy.Create<Garbage>() };

            //Act
            var action = () => Instance.InsertMany(entities);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TryingToInsertNulls, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenIdIsChangedByAbastractCreateMethod_Throw()
        {
            //Arrange
            var bogus = new BogusGarbageBundleRepository();
            var entities = new List<Garbage> { Dummy.Create<Garbage>(), Dummy.Create<Garbage>() };

            //Act
            var action = () => bogus.InsertMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.IdWasChangedBeforeInsert, nameof(Garbage), 5, 4));
        }

        [TestMethod]
        public void WhenNoEntityIsNullAndNoIdIsModifiedByAbstractCreateMethod_AddAllByIncrementingId()
        {
            //Arrange
            var items = Dummy.CreateMany<Garbage>(3).ToList();

            //Act
            Instance.InsertMany(items);

            //Assert
            Instance.Contains(x => x == items[0] with { Id = 5 });
            Instance.Contains(x => x == items[1] with { Id = 6 });
            Instance.Contains(x => x == items[2] with { Id = 7 });
        }
    }

    [TestClass]
    public class Delete_Entity : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenEntityIsNull_Throw()
        {
            //Arrange
            Garbage entity = null!;

            //Act
            var action = () => Instance.Delete(entity);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entity));
        }

        [TestMethod]
        public void WhenEntityIsNotInRepository_Throw()
        {
            //Arrange
            var entity = Dummy.Create<Garbage>();

            //Act
            var action = () => Instance.Delete(entity);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenEntityExistsInRepository_RemoveIt()
        {
            //Arrange
            var item = GarbageBundleRepository.Items.GetRandom()!;

            //Act
            Instance.Delete(item);

            //Assert
            Instance.Contains(item).Should().BeFalse();
        }
    }

    [TestClass]
    public class Delete_Predicate : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Garbage, bool> predicate = null!;

            //Act
            var action = () => Instance.Delete(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
        }

        [TestMethod]
        public void WhenPredicateYieldsNoResult_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Delete(x => x.Name.Contains("Kevin"));

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenPredicateRefersToOneExistingItem_RemoveThatItem()
        {
            //Arrange
            var item = GarbageBundleRepository.Items.GetRandom()!;

            //Act
            Instance.Delete(x => x.Name == item.Name);

            //Assert
            Instance.Contains(item).Should().BeFalse();
        }

        [TestMethod]
        public void WhenPredicateRefersToMultipleExistingItems_RemoveAllOfThem()
        {
            //Arrange

            //Act
            Instance.Delete(x => x.Name.Contains("er"));

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }
    }

    [TestClass]
    public class TryDelete_Entity : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenEntityIsNull_Throw()
        {
            //Arrange
            Garbage entity = null!;

            //Act
            var action = () => Instance.TryDelete(entity);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entity));
        }

        [TestMethod]
        public void WhenEntityIsNotInRepository_DoNotThrow()
        {
            //Arrange
            var entity = Dummy.Create<Garbage>();

            //Act
            var action = () => Instance.TryDelete(entity);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenEntityExistsInRepository_RemoveIt()
        {
            //Arrange
            var item = GarbageBundleRepository.Items.GetRandom()!;

            //Act
            Instance.TryDelete(item);

            //Assert
            Instance.Contains(item).Should().BeFalse();
        }
    }

    [TestClass]
    public class TryDelete_Predicate : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Garbage, bool> predicate = null!;

            //Act
            var action = () => Instance.TryDelete(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
        }

        [TestMethod]
        public void WhenPredicateYieldsNoResult_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryDelete(x => x.Name.Contains("Kevin"));

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenPredicateRefersToOneExistingItem_RemoveThatItem()
        {
            //Arrange
            var item = GarbageBundleRepository.Items.GetRandom()!;

            //Act
            Instance.TryDelete(x => x.Name == item.Name);

            //Assert
            Instance.Contains(item).Should().BeFalse();
        }

        [TestMethod]
        public void WhenPredicateRefersToMultipleExistingItems_RemoveAllOfThem()
        {
            //Arrange

            //Act
            Instance.TryDelete(x => x.Name.Contains("er"));

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }
    }

    [TestClass]
    public class DeleteMany_Params : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.DeleteMany();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToDelete, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenContainsNulls_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.DeleteMany(Dummy.Create<Garbage>(), null!, Dummy.Create<DerivedGarbage>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteNulls, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenOneEntityIsNotInRepository_Throw()
        {
            //Arrange
            var entities = GarbageBundleRepository.Items.GetManyRandoms(3).Concat(Dummy.Create<Garbage>()).ToArray();

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenThrowsAfterDeletingExistingElements_DoNotApplyChanges()
        {
            //Arrange
            var existingEntities = GarbageBundleRepository.Items.GetManyRandoms(3).ToList();
            var entities = existingEntities.Concat(Dummy.Create<Garbage>()).ToArray();

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<Exception>();
            Instance.FetchAll().Should().Contain(existingEntities);
        }

        [TestMethod]
        public void WhenContainsOneEntityThatIsInRepo_RemoveFromRepo()
        {
            //Arrange
            var entity = GarbageBundleRepository.Items.GetRandom()!;

            //Act
            Instance.DeleteMany(entity);

            //Assert
            Instance.Contains(entity).Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainMultipleEntitiesFromRepo_RemoveAllEntities()
        {
            //Arrange
            var entities = GarbageBundleRepository.Items.GetManyRandoms(2).ToArray();

            //Act
            Instance.DeleteMany(entities);

            //Assert
            Instance.Contains(entities).Should().BeFalse();
        }
    }

    [TestClass]
    public class DeleteMany_Enumerable : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesNull_Throw()
        {
            //Arrange
            IEnumerable<Garbage> entities = null!;

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entities));
        }

        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.DeleteMany(new List<Garbage>());

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToDelete, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenContainsNulls_Throw()
        {
            //Arrange
            var entities = new List<Garbage> { Dummy.Create<Garbage>(), null!, Dummy.Create<DerivedGarbage>() };

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteNulls, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenOneEntityIsNotInRepository_Throw()
        {
            //Arrange
            var entities = GarbageBundleRepository.Items.GetManyRandoms(3).Concat(Dummy.Create<Garbage>()).ToList();

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenThrowsAfterDeletingExistingElements_DoNotApplyChanges()
        {
            //Arrange
            var existingEntities = GarbageBundleRepository.Items.GetManyRandoms(3).ToList();
            var entities = existingEntities.Concat(Dummy.Create<Garbage>()).ToList();

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<Exception>();
            Instance.FetchAll().Should().Contain(existingEntities);
        }

        [TestMethod]
        public void WhenContainsOneEntityThatIsInRepo_RemoveFromRepo()
        {
            //Arrange
            var entity = GarbageBundleRepository.Items.GetRandom()!;

            //Act
            Instance.DeleteMany(new List<Garbage> { entity });

            //Assert
            Instance.Contains(entity).Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainMultipleEntitiesFromRepo_RemoveAllEntities()
        {
            //Arrange
            var entities = GarbageBundleRepository.Items.GetManyRandoms(2).ToList();

            //Act
            Instance.DeleteMany(entities);

            //Assert
            Instance.Contains(entities).Should().BeFalse();
        }
    }

    [TestClass]
    public class TryDeleteMany_Params : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.TryDeleteMany();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToDelete, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenContainsNulls_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryDeleteMany(Dummy.Create<Garbage>(), null!, Dummy.Create<DerivedGarbage>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenOneEntityIsNotInRepository_Throw()
        {
            //Arrange
            var entities = GarbageBundleRepository.Items.GetManyRandoms(3).Concat(Dummy.Create<Garbage>()).ToArray();

            //Act
            var action = () => Instance.TryDeleteMany(entities);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThrowsAfterDeletingExistingElements_ApplyChangesForExistingEntities()
        {
            //Arrange
            var existingEntities = GarbageBundleRepository.Items.GetManyRandoms(3).ToList();
            var entities = existingEntities.Concat(Dummy.Create<Garbage>()).ToArray();

            //Act
            Instance.TryDeleteMany(entities);

            //Assert
            Instance.FetchAll().Should().NotContain(existingEntities);
        }

        [TestMethod]
        public void WhenContainsOneEntityThatIsInRepo_RemoveFromRepo()
        {
            //Arrange
            var entity = GarbageBundleRepository.Items.GetRandom()!;

            //Act
            Instance.TryDeleteMany(entity);

            //Assert
            Instance.Contains(entity).Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainMultipleEntitiesFromRepo_RemoveAllEntities()
        {
            //Arrange
            var entities = GarbageBundleRepository.Items.GetManyRandoms(2).ToArray();

            //Act
            Instance.TryDeleteMany(entities);

            //Assert
            Instance.Contains(entities).Should().BeFalse();
        }
    }

    [TestClass]
    public class TryDeleteMany_Enumerable : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesNull_Throw()
        {
            //Arrange
            IEnumerable<Garbage> entities = null!;

            //Act
            var action = () => Instance.TryDeleteMany(entities);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entities));
        }

        [TestMethod]
        public void WhenNoEntityProvided_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryDeleteMany(new List<Garbage>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenContainsNulls_DoNotThrow()
        {
            //Arrange
            var entities = new List<Garbage> { Dummy.Create<Garbage>(), null!, Dummy.Create<DerivedGarbage>() };

            //Act
            var action = () => Instance.TryDeleteMany(entities);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenOneEntityIsNotInRepository_DoNotThrow()
        {
            //Arrange
            var entities = GarbageBundleRepository.Items.GetManyRandoms(3).Concat(Dummy.Create<Garbage>()).ToList();

            //Act
            var action = () => Instance.TryDeleteMany(entities);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThrowsAfterDeletingExistingElements_ApplyChangesForExistingEntities()
        {
            //Arrange
            var existingEntities = GarbageBundleRepository.Items.GetManyRandoms(3).ToList();
            var entities = existingEntities.Concat(Dummy.Create<Garbage>()).ToList();

            //Act
            Instance.TryDeleteMany(entities);

            //Assert
            Instance.FetchAll().Should().NotContain(existingEntities);
        }

        [TestMethod]
        public void WhenContainsOneEntityThatIsInRepo_RemoveFromRepo()
        {
            //Arrange
            var entity = GarbageBundleRepository.Items.GetRandom()!;

            //Act
            Instance.TryDeleteMany(new List<Garbage> { entity });

            //Assert
            Instance.Contains(entity).Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainMultipleEntitiesFromRepo_RemoveAllEntities()
        {
            //Arrange
            var entities = GarbageBundleRepository.Items.GetManyRandoms(2).ToList();

            //Act
            Instance.TryDeleteMany(entities);

            //Assert
            Instance.Contains(entities).Should().BeFalse();
        }
    }

    [TestClass]
    public class DeleteById : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.DeleteById(-Dummy.Create<int>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenUsingExistingId_RemoveEntityWithId()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.GetRandom()!.Id;

            //Act
            Instance.DeleteById(id);

            //Assert
            Instance.Contains(x => x.Id == id).Should().BeFalse();
        }
    }

    [TestClass]
    public class DeleteManyById_Params : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_Throw()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>();

            //Act
            var action = () => Instance.DeleteManyById(1, 3, id);

            //Assert
            action.Should().Throw<Exception>(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Garbage), id));
        }

        [TestMethod]
        public void WhenThrowing_DoNotUpdateRepository()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>();

            //Act
            var action = () => Instance.DeleteManyById(1, 3, id);

            //Assert
            action.Should().Throw<Exception>();
            Instance.FetchAll().Should().BeEquivalentTo(GarbageBundleRepository.Items);
        }

        [TestMethod]
        public void WhenUsingExistingIds_RemoveAll()
        {
            //Arrange

            //Act
            Instance.DeleteManyById(1, 4);

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Garbage>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }
    }

    [TestClass]
    public class DeleteManyById_Enumerable : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenIdsNull_Throw()
        {
            //Arrange
            IEnumerable<int> ids = null!;

            //Act
            var action = () => Instance.DeleteManyById(ids);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(ids));
        }

        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_Throw()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>();

            //Act
            var action = () => Instance.DeleteManyById(new List<int> { 1, 3, id });

            //Assert
            action.Should().Throw<Exception>(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Garbage), id));
        }

        [TestMethod]
        public void WhenThrowing_DoNotUpdateRepository()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>();

            //Act
            var action = () => Instance.DeleteManyById(new List<int> { 1, 3, id });

            //Assert
            action.Should().Throw<Exception>();
            Instance.FetchAll().Should().BeEquivalentTo(GarbageBundleRepository.Items);
        }

        [TestMethod]
        public void WhenUsingExistingIds_RemoveAll()
        {
            //Arrange

            //Act
            Instance.DeleteManyById(new List<int> { 1, 4 });

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Garbage>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }
    }

    [TestClass]
    public class TryDeleteById : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryDeleteById(-Dummy.Create<int>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenUsingExistingId_RemoveEntityWithId()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.GetRandom()!.Id;

            //Act
            Instance.TryDeleteById(id);

            //Assert
            Instance.Contains(x => x.Id == id).Should().BeFalse();
        }
    }

    [TestClass]
    public class TryDeleteManyById_Params : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_DoNotThrow()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>();

            //Act
            var action = () => Instance.TryDeleteManyById(1, 3, id);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_RemoveExistingOnes()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>();

            //Act
            Instance.TryDeleteManyById(1, 3, id);

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenUsingExistingIds_RemoveAll()
        {
            //Arrange

            //Act
            Instance.TryDeleteManyById(1, 4);

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Garbage>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }
    }

    [TestClass]
    public class TryDeleteManyById_Enumerable : Tester<GarbageBundleRepository>
    {
        [TestMethod]
        public void WhenIdsNull_Throw()
        {
            //Arrange
            IEnumerable<int> ids = null!;

            //Act
            var action = () => Instance.TryDeleteManyById(ids);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(ids));
        }

        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_DoNotThrow()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>();

            //Act
            var action = () => Instance.TryDeleteManyById(new List<int> { 1, 3, id });

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_RemoveExistingOnes()
        {
            //Arrange
            var id = GarbageBundleRepository.Items.Max(x => x.Id) + Dummy.Create<short>();

            //Act
            Instance.TryDeleteManyById(new List<int> { 1, 3, id });

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenUsingExistingIds_RemoveAll()
        {
            //Arrange

            //Act
            Instance.TryDeleteManyById(new List<int> { 1, 4 });

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Garbage>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }
    }
}