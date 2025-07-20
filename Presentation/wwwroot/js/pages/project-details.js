$('#itemModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const id = button.data('bs-id')
    const type = button.data('bs-type')
    const modal = $(this)
    let el = ``;
    let typeNum = type == "Estimate" ? 0 : 1
    items.filter(i => i.ProjectDetailId == id && i.Type == typeNum).forEach(item => {
        el += `<tr>
            <td>${item.Title}</td>
            <td>${item.Area}</td>
            <td>${item.UnitPrice}</td>
            <td>${item.TotalPrice}</td>
            <td></td>
        </tr>`
    })
    modal.find('table tbody').html(el)
    
    modal.find('[name=ProjectDetailId]').val(id)
    modal.find('[name=Type]').val(type)
})
$('[name=UnitPrice], [name=Area]').on('keyup', function(){
    
    let unit = $('[name=UnitPrice]').val() ?? 0;
    let area = $('[name=Area]').val() ?? 0;
    $('[name=TotalPrice]').val(unit * area);
    
})