﻿using Microsoft.AspNetCore.Components.Forms;

namespace NetCorePal.D3Shop.Web.Admin.Client.Components.Identity.Role;

public partial class AddRole
{
    [Inject] private IRolesService RolesService { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;

    [Parameter] public EventCallback OnItemAdded { get; set; }

    private Form<CreateRoleRequest> _form = default!;

    private Tabs _tabs = default!;

    private List<string> _assignedPermissionCodes = [];

    private CreateRoleRequest _newRoleModel = new();

    private bool _modalVisible;
    private bool _modalConfirmLoading;

    private void ShowModal()
    {
        _modalVisible = true;
    }


    private void CloseModal()
    {
        _modalVisible = false;
        _newRoleModel = new CreateRoleRequest();
        _assignedPermissionCodes.Clear();
        _tabs.GoTo(0);
    }

    private async Task Form_OnFinish(EditContext editContext)
    {
        _modalConfirmLoading = true;
        StateHasChanged();
       // _newRoleModel.PermissionCodes = _assignedPermissionCodes;
        var response = await RolesService.CreateRole(_newRoleModel);
        if (response.Success)
        {
            _ = Message.Success("创建成功！");
            CloseModal();
            await OnItemAdded.InvokeAsync();
        }
        else
        {
            _ = Message.Error(response.Message);
        }

        _modalConfirmLoading = false;
    }

    private void Form_OnFinishFailed(EditContext editContext)
    {
        _tabs.GoTo(0);
    }
}