$('#detailModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const id = button.data('bs-id')
    const costGroup = button.data('bs-cost-group')
    const creditor = button.data('bs-creditor')
    const description = button.data('bs-description')
    const date = button.data('bs-date')
    const paidAmount= button.data('bs-paid-amount')
    const activityId = button.data('bs-activity-id')
    const unverifiedInvoiceId = button.data('bs-unverified-invoice-id')
    const modal = $(this)
    const modelFiles = files.filter(i => i.ModelId == activityId || i.ModelId == unverifiedInvoiceId);
    const modelPayoffs = payoffs.filter(i => i.UnsettledInvoiceId == id);
    let payoffsEl = ``;
    let el =``;
    modelPayoffs.forEach(item => {
        payoffsEl += `
            <tr>
                <td>${item.TypeDisplay}</td>
                <td>${item.Number}</td>
                <td>${item.Date}</td>
                <td colspan="2">${item.Price}</td>
            </tr>
        `
    })
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
                </div>`
    })
    modal.find('.images').html(el)
    modal.find('#payoffs-table').html(payoffsEl)
    modal.find('#description-el').text(description)
    modal.find('#cost-group-el').text(costGroup)
    modal.find('#paid-amount-el').text(paidAmount)
    modal.find('#date-el').text(date)
    modal.find('#creditor-el').text(creditor)
    modal.find('.id').val(id)
})