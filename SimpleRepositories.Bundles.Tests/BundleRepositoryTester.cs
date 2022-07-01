namespace SimpleRepositories.Bundles.Tests;

[TestClass]
public class BundleRepositoryTester
{
    [TestClass]
    public class Update : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenEntityIsNull_Throw()
        {
            //Arrange
            Dummy entity = null!;

            //Act
            var action = () => Instance.Update(entity);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entity));
        }

        [TestMethod]
        public void WhenEntityIsNotPartOfRepository_Throw()
        {
            //Arrange
            var entity = Fixture.Build<Dummy>().With(x => x.Id, DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>()).Create();

            //Act
            var action = () => Instance.Update(entity);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Dummy), entity.Id));
        }

        [TestMethod]
        public void WhenEntityAlreadyExistsInRepository_UpdateIt()
        {
            //Arrange
            var entity = new Dummy
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
    public class UpdateMany_Params : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.UpdateMany();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToUpdate, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenContainsNullEntities_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.UpdateMany(DummyBundleRepository.Items[0] with { Name = "Bogus" }, null!, DummyBundleRepository.Items[1] with { Name = "Not gonna work" });

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TryingToUpdateNulls, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenEntityIsNotPartOfRepository_Throw()
        {
            //Arrange
            var entity = Fixture.Build<Dummy>().With(x => x.Id, DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>()).Create();

            //Act
            var action = () => Instance.UpdateMany(entity);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Dummy), entity.Id));
        }

        [TestMethod]
        public void WhenEntityAlreadyExistsInRepository_UpdateIt()
        {
            //Arrange
            var entity = new Dummy
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
            var entity1 = DummyBundleRepository.Items[1] with { Name = "Not Jerry" };
            var entity2 = DummyBundleRepository.Items[2] with { Name = "Not Terry" };

            //Act
            Instance.UpdateMany(entity1, entity2);

            //Assert
            Instance.FetchById(1).Should().Be(entity1);
            Instance.FetchById(3).Should().Be(entity2);
        }
    }

    [TestClass]
    public class UpdateMany_Enumerable : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesNull_Throw()
        {
            //Arrange
            IEnumerable<Dummy> entities = null!;

            //Act
            var action = () => Instance.UpdateMany(entities);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entities));
        }

        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange
            var entities = new List<Dummy>();

            //Act
            var action = () => Instance.UpdateMany(entities);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToUpdate, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenContainsNullEntities_Throw()
        {
            //Arrange
            var entities = new List<Dummy>
            {
                DummyBundleRepository.Items[0] with {Name = "Bogus"},
                null!,
                DummyBundleRepository.Items[1] with {Name = "Not gonna work"},
            };

            //Act
            var action = () => Instance.UpdateMany(entities);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TryingToUpdateNulls, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenEntityIsNotPartOfRepository_Throw()
        {
            //Arrange
            var entities = new List<Dummy> { Fixture.Build<Dummy>().With(x => x.Id, DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>()).Create() };

            //Act
            var action = () => Instance.UpdateMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Dummy), entities.Single().Id));
        }

        [TestMethod]
        public void WhenEntityAlreadyExistsInRepository_UpdateIt()
        {
            //Arrange
            var entities = new List<Dummy>
            {
                new Dummy
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
            var entities = new List<Dummy>
            {
                DummyBundleRepository.Items[1] with { Name = "Not Jerry" },
                DummyBundleRepository.Items[2] with { Name = "Not Terry" }
            };

            //Act
            Instance.UpdateMany(entities);

            //Assert
            Instance.FetchById(1).Should().Be(entities[0]);
            Instance.FetchById(3).Should().Be(entities[1]);
        }
    }

    [TestClass]
    public class Insert : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenEntityIsNull_Throw()
        {
            //Arrange
            Dummy entity = null!;

            //Act
            var action = () => Instance.Insert(entity);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entity));
        }

        [TestMethod]
        public void WhenIdIsChangedByAbstractCreateMethod_Throw()
        {
            //Arrange
            var bogus = new BogusDummyBundleRepository();

            //Act
            var action = () => bogus.Insert(Fixture.Create<Dummy>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.IdWasChangedBeforeInsert, nameof(Dummy), 5, 4));
        }

        [TestMethod]
        public void WhenAbstractMethodIsNotBogus_InsertAtEndWithAutoIncrementedId()
        {
            //Arrange
            var entity = Fixture.Create<Dummy>();

            //Act
            Instance.Insert(entity);

            //Assert
            Instance.Contains(x => x == entity with { Id = 5 });
        }
    }

    [TestClass]
    public class InsertMany_Params : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.InsertMany();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToInsert, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenOneEntityIsNull_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.InsertMany(Fixture.Create<Dummy>(), null!, Fixture.Create<Dummy>());

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TryingToInsertNulls, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenIdIsChangedByAbastractCreateMethod_Throw()
        {
            //Arrange
            var bogus = new BogusDummyBundleRepository();

            //Act
            var action = () => bogus.InsertMany(Fixture.Create<Dummy>(), Fixture.Create<Dummy>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.IdWasChangedBeforeInsert, nameof(Dummy), 5, 4));
        }

        [TestMethod]
        public void WhenNoEntityIsNullAndNoIdIsModifiedByAbstractCreateMethod_AddAllByIncrementingId()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(3).ToArray();

            //Act
            Instance.InsertMany(items);

            //Assert
            Instance.Contains(x => x == items[0] with { Id = 5 });
            Instance.Contains(x => x == items[1] with { Id = 6 });
            Instance.Contains(x => x == items[2] with { Id = 7 });
        }
    }

    [TestClass]
    public class InsertMany_Enumerable : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesIsNull_Throw()
        {
            //Arrange
            IEnumerable<Dummy> entities = null!;

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
            var action = () => Instance.InsertMany(new List<Dummy>());

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToInsert, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenOneEntityIsNull_Throw()
        {
            //Arrange
            var entities = new List<Dummy> { Fixture.Create<Dummy>(), null!, Fixture.Create<Dummy>() };

            //Act
            var action = () => Instance.InsertMany(entities);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TryingToInsertNulls, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenIdIsChangedByAbastractCreateMethod_Throw()
        {
            //Arrange
            var bogus = new BogusDummyBundleRepository();
            var entities = new List<Dummy> { Fixture.Create<Dummy>(), Fixture.Create<Dummy>() };

            //Act
            var action = () => bogus.InsertMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.IdWasChangedBeforeInsert, nameof(Dummy), 5, 4));
        }

        [TestMethod]
        public void WhenNoEntityIsNullAndNoIdIsModifiedByAbstractCreateMethod_AddAllByIncrementingId()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(3).ToList();

            //Act
            Instance.InsertMany(items);

            //Assert
            Instance.Contains(x => x == items[0] with { Id = 5 });
            Instance.Contains(x => x == items[1] with { Id = 6 });
            Instance.Contains(x => x == items[2] with { Id = 7 });
        }
    }

    [TestClass]
    public class Delete_Entity : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenEntityIsNull_Throw()
        {
            //Arrange
            Dummy entity = null!;

            //Act
            var action = () => Instance.Delete(entity);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entity));
        }

        [TestMethod]
        public void WhenEntityIsNotInRepository_Throw()
        {
            //Arrange
            var entity = Fixture.Create<Dummy>();

            //Act
            var action = () => Instance.Delete(entity);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenEntityExistsInRepository_RemoveIt()
        {
            //Arrange
            var item = DummyBundleRepository.Items.GetRandom()!;

            //Act
            Instance.Delete(item);

            //Assert
            Instance.Contains(item).Should().BeFalse();
        }
    }

    [TestClass]
    public class Delete_Predicate : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;

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
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenPredicateRefersToOneExistingItem_RemoveThatItem()
        {
            //Arrange
            var item = DummyBundleRepository.Items.GetRandom()!;

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
            Instance.FetchAll().Should().BeEquivalentTo(new List<Dummy>
            {
                new DerivedDummy
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedDummy
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
    public class TryDelete_Entity : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenEntityIsNull_Throw()
        {
            //Arrange
            Dummy entity = null!;

            //Act
            var action = () => Instance.TryDelete(entity);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entity));
        }

        [TestMethod]
        public void WhenEntityIsNotInRepository_DoNotThrow()
        {
            //Arrange
            var entity = Fixture.Create<Dummy>();

            //Act
            var action = () => Instance.TryDelete(entity);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenEntityExistsInRepository_RemoveIt()
        {
            //Arrange
            var item = DummyBundleRepository.Items.GetRandom()!;

            //Act
            Instance.TryDelete(item);

            //Assert
            Instance.Contains(item).Should().BeFalse();
        }
    }

    [TestClass]
    public class TryDelete_Predicate : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;

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
            var item = DummyBundleRepository.Items.GetRandom()!;

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
            Instance.FetchAll().Should().BeEquivalentTo(new List<Dummy>
            {
                new DerivedDummy
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedDummy
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
    public class DeleteMany_Params : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.DeleteMany();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToDelete, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenContainsNulls_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.DeleteMany(Fixture.Create<Dummy>(), null!, Fixture.Create<DerivedDummy>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteNulls, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenOneEntityIsNotInRepository_Throw()
        {
            //Arrange
            var entities = DummyBundleRepository.Items.GetManyRandoms(3).Concat(Fixture.Create<Dummy>()).ToArray();

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenThrowsAfterDeletingExistingElements_DoNotApplyChanges()
        {
            //Arrange
            var existingEntities = DummyBundleRepository.Items.GetManyRandoms(3).ToList();
            var entities = existingEntities.Concat(Fixture.Create<Dummy>()).ToArray();

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
            var entity = DummyBundleRepository.Items.GetRandom()!;

            //Act
            Instance.DeleteMany(entity);

            //Assert
            Instance.Contains(entity).Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainMultipleEntitiesFromRepo_RemoveAllEntities()
        {
            //Arrange
            var entities = DummyBundleRepository.Items.GetManyRandoms(2).ToArray();

            //Act
            Instance.DeleteMany(entities);

            //Assert
            Instance.Contains(entities).Should().BeFalse();
        }
    }

    [TestClass]
    public class DeleteMany_Enumerable : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesNull_Throw()
        {
            //Arrange
            IEnumerable<Dummy> entities = null!;

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
            var action = () => Instance.DeleteMany(new List<Dummy>());

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToDelete, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenContainsNulls_Throw()
        {
            //Arrange
            var entities = new List<Dummy> { Fixture.Create<Dummy>(), null!, Fixture.Create<DerivedDummy>() };

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteNulls, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenOneEntityIsNotInRepository_Throw()
        {
            //Arrange
            var entities = DummyBundleRepository.Items.GetManyRandoms(3).Concat(Fixture.Create<Dummy>()).ToList();

            //Act
            var action = () => Instance.DeleteMany(entities);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenThrowsAfterDeletingExistingElements_DoNotApplyChanges()
        {
            //Arrange
            var existingEntities = DummyBundleRepository.Items.GetManyRandoms(3).ToList();
            var entities = existingEntities.Concat(Fixture.Create<Dummy>()).ToList();

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
            var entity = DummyBundleRepository.Items.GetRandom()!;

            //Act
            Instance.DeleteMany(new List<Dummy> { entity });

            //Assert
            Instance.Contains(entity).Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainMultipleEntitiesFromRepo_RemoveAllEntities()
        {
            //Arrange
            var entities = DummyBundleRepository.Items.GetManyRandoms(2).ToList();

            //Act
            Instance.DeleteMany(entities);

            //Assert
            Instance.Contains(entities).Should().BeFalse();
        }
    }

    [TestClass]
    public class TryDeleteMany_Params : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityProvided_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.TryDeleteMany();

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.NoEntityToDelete, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenContainsNulls_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryDeleteMany(Fixture.Create<Dummy>(), null!, Fixture.Create<DerivedDummy>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenOneEntityIsNotInRepository_Throw()
        {
            //Arrange
            var entities = DummyBundleRepository.Items.GetManyRandoms(3).Concat(Fixture.Create<Dummy>()).ToArray();

            //Act
            var action = () => Instance.TryDeleteMany(entities);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThrowsAfterDeletingExistingElements_ApplyChangesForExistingEntities()
        {
            //Arrange
            var existingEntities = DummyBundleRepository.Items.GetManyRandoms(3).ToList();
            var entities = existingEntities.Concat(Fixture.Create<Dummy>()).ToArray();

            //Act
            Instance.TryDeleteMany(entities);

            //Assert
            Instance.FetchAll().Should().NotContain(existingEntities);
        }

        [TestMethod]
        public void WhenContainsOneEntityThatIsInRepo_RemoveFromRepo()
        {
            //Arrange
            var entity = DummyBundleRepository.Items.GetRandom()!;

            //Act
            Instance.TryDeleteMany(entity);

            //Assert
            Instance.Contains(entity).Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainMultipleEntitiesFromRepo_RemoveAllEntities()
        {
            //Arrange
            var entities = DummyBundleRepository.Items.GetManyRandoms(2).ToArray();

            //Act
            Instance.TryDeleteMany(entities);

            //Assert
            Instance.Contains(entities).Should().BeFalse();
        }
    }

    [TestClass]
    public class TryDeleteMany_Enumerable : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesNull_Throw()
        {
            //Arrange
            IEnumerable<Dummy> entities = null!;

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
            var action = () => Instance.TryDeleteMany(new List<Dummy>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenContainsNulls_DoNotThrow()
        {
            //Arrange
            var entities = new List<Dummy> { Fixture.Create<Dummy>(), null!, Fixture.Create<DerivedDummy>() };

            //Act
            var action = () => Instance.TryDeleteMany(entities);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenOneEntityIsNotInRepository_DoNotThrow()
        {
            //Arrange
            var entities = DummyBundleRepository.Items.GetManyRandoms(3).Concat(Fixture.Create<Dummy>()).ToList();

            //Act
            var action = () => Instance.TryDeleteMany(entities);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThrowsAfterDeletingExistingElements_ApplyChangesForExistingEntities()
        {
            //Arrange
            var existingEntities = DummyBundleRepository.Items.GetManyRandoms(3).ToList();
            var entities = existingEntities.Concat(Fixture.Create<Dummy>()).ToList();

            //Act
            Instance.TryDeleteMany(entities);

            //Assert
            Instance.FetchAll().Should().NotContain(existingEntities);
        }

        [TestMethod]
        public void WhenContainsOneEntityThatIsInRepo_RemoveFromRepo()
        {
            //Arrange
            var entity = DummyBundleRepository.Items.GetRandom()!;

            //Act
            Instance.TryDeleteMany(new List<Dummy> { entity });

            //Assert
            Instance.Contains(entity).Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainMultipleEntitiesFromRepo_RemoveAllEntities()
        {
            //Arrange
            var entities = DummyBundleRepository.Items.GetManyRandoms(2).ToList();

            //Act
            Instance.TryDeleteMany(entities);

            //Assert
            Instance.Contains(entities).Should().BeFalse();
        }
    }

    [TestClass]
    public class DeleteById : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.DeleteById(-Fixture.Create<int>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.TryingToDeleteInexistantEntities, nameof(Dummy)));
        }

        [TestMethod]
        public void WhenUsingExistingId_RemoveEntityWithId()
        {
            //Arrange
            var id = DummyBundleRepository.Items.GetRandom()!.Id;

            //Act
            Instance.DeleteById(id);

            //Assert
            Instance.Contains(x => x.Id == id).Should().BeFalse();
        }
    }

    [TestClass]
    public class DeleteManyById_Params : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_Throw()
        {
            //Arrange
            var id = DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>();

            //Act
            var action = () => Instance.DeleteManyById(1, 3, id);

            //Assert
            action.Should().Throw<Exception>(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Dummy), id));
        }

        [TestMethod]
        public void WhenThrowing_DoNotUpdateRepository()
        {
            //Arrange
            var id = DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>();

            //Act
            var action = () => Instance.DeleteManyById(1, 3, id);

            //Assert
            action.Should().Throw<Exception>();
            Instance.FetchAll().Should().BeEquivalentTo(DummyBundleRepository.Items);
        }

        [TestMethod]
        public void WhenUsingExistingIds_RemoveAll()
        {
            //Arrange

            //Act
            Instance.DeleteManyById(1, 4);

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Dummy>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
                new DerivedDummy
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
    public class DeleteManyById_Enumerable : Tester<DummyBundleRepository>
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
            var id = DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>();

            //Act
            var action = () => Instance.DeleteManyById(new List<int> { 1, 3, id });

            //Assert
            action.Should().Throw<Exception>(string.Format(Exceptions.NoEntityFoundToUpdate, nameof(Dummy), id));
        }

        [TestMethod]
        public void WhenThrowing_DoNotUpdateRepository()
        {
            //Arrange
            var id = DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>();

            //Act
            var action = () => Instance.DeleteManyById(new List<int> { 1, 3, id });

            //Assert
            action.Should().Throw<Exception>();
            Instance.FetchAll().Should().BeEquivalentTo(DummyBundleRepository.Items);
        }

        [TestMethod]
        public void WhenUsingExistingIds_RemoveAll()
        {
            //Arrange

            //Act
            Instance.DeleteManyById(new List<int> { 1, 4 });

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Dummy>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
                new DerivedDummy
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
    public class TryDeleteById : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryDeleteById(-Fixture.Create<int>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenUsingExistingId_RemoveEntityWithId()
        {
            //Arrange
            var id = DummyBundleRepository.Items.GetRandom()!.Id;

            //Act
            Instance.TryDeleteById(id);

            //Assert
            Instance.Contains(x => x.Id == id).Should().BeFalse();
        }
    }

    [TestClass]
    public class TryDeleteManyById_Params : Tester<DummyBundleRepository>
    {
        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_DoNotThrow()
        {
            //Arrange
            var id = DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>();

            //Act
            var action = () => Instance.TryDeleteManyById(1, 3, id);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_RemoveExistingOnes()
        {
            //Arrange
            var id = DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>();

            //Act
            Instance.TryDeleteManyById(1, 3, id);

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Dummy>
            {
                new DerivedDummy
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedDummy
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
            Instance.FetchAll().Should().BeEquivalentTo(new List<Dummy>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
                new DerivedDummy
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
    public class TryDeleteManyById_Enumerable : Tester<DummyBundleRepository>
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
            var id = DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>();

            //Act
            var action = () => Instance.TryDeleteManyById(new List<int> { 1, 3, id });

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenTryingToDeleteNonExistantId_RemoveExistingOnes()
        {
            //Arrange
            var id = DummyBundleRepository.Items.Max(x => x.Id) + Fixture.Create<short>();

            //Act
            Instance.TryDeleteManyById(new List<int> { 1, 3, id });

            //Assert
            Instance.FetchAll().Should().BeEquivalentTo(new List<Dummy>
            {
                new DerivedDummy
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedDummy
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
            Instance.FetchAll().Should().BeEquivalentTo(new List<Dummy>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
                new DerivedDummy
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