$("#updateModal").on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const id = button.data('id')
    const modelFiles = files.filter(i => i.ModelId == id);
    let el = ``;
    modelFiles.forEach(item => {
        el += `
                <div class="col-3">
                    <div class="card">
                        <img src="${url + item.SavedName}" class="card-img-top"
                             alt=""/>
                        <div class="card-body">
                            <p class="card-text">${item.Description}</p>
                            <a class="btn btn-danger">حذف</a>
                        </div>

                    </div>
                </div>
`
    })
    const projectId = button.data('projectId')
    const creditor = button.data('creditorId')
    const costGroup = button.data('costGroupId')
    const date = button.data('date')
    const amount = button.data('amount')
    const description = button.data('description')
    const modal = $(this)
    modal.find('.images').html(el)
    modal.find('input[name=Id]').val(id)
    modal.find('input[name=Date]').val(date)
    modal.find('input[name=Amount]').val(amount)
    modal.find('textarea[name=Description]').val(description)
    modal.find('select[name=CreditorId]')
        .val(creditor)
        .trigger("change")
    modal.find('select[name=CostGroupId]')
        .val(costGroup)
        .trigger("change")
    modal.find('select[name=ProjectId]')
        .val(projectId)
        .trigger("change")
})
