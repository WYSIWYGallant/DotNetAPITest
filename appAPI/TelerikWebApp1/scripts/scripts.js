function openNewWindow(personId) {
    var url = 'Page2.aspx?PersonID=' + personId;
    window.open(url, '_blank', 'resizable=yes,width=800,height=600');
}