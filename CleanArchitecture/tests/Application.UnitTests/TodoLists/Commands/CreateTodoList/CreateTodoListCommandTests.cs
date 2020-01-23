﻿using ca_sln_2.Application.TodoLists.Commands.CreateTodoList;
using ca_sln_2.Application.UnitTests.Common;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ca_sln_2.Application.UnitTests.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_ShouldPersistTodoList()
        {
            var command = new CreateTodoListCommand
            {
                Title = "Bucket List"
            };

            var handler = new CreateTodoListCommand.CreateTodoListCommandHandler(Context);

            var result = await handler.Handle(command, CancellationToken.None);

            var entity = Context.TodoLists.Find(result);

            entity.ShouldNotBeNull();
            entity.Title.ShouldBe(command.Title);
        }
    }
}
