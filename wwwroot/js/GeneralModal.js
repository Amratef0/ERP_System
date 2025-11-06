
function openModalWithAjax(url, title = "Details") {
    const modal = new bootstrap.Modal(document.getElementById('generalModal'));
    document.getElementById('generalModalLabel').innerText = title;
    document.getElementById('generalModalBody').innerHTML = `
        <div class="text-center">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>`;

    fetch(url, { headers: { "X-Requested-With": "XMLHttpRequest" } })
        .then(response => response.text())
        .then(html => {
            document.getElementById('generalModalBody').innerHTML = html;
        })
        .catch(error => {
            document.getElementById('generalModalBody').innerHTML =
                `<div class="alert alert-danger">Error loading content: ${error}</div>`;
        });

    modal.show();
}

const modalEl = document.getElementById('generalModal');

modalEl.addEventListener('shown.bs.modal', () => {
    const shine = modalEl.querySelector('.shine-overlay');
    if (shine) {
        shine.style.animation = 'none';   // reset
        void shine.offsetWidth;           // force reflow
        shine.style.animation = 'shine 3s infinite linear'; // restart animation
    }
});

modalEl.addEventListener('hidden.bs.modal', () => {
    const shine = modalEl.querySelector('.shine-overlay');
    if (shine) shine.style.animation = 'none'; // stop animation
});


function showConfirm(message, onYes) {
    const confirmMessage = document.getElementById("confirmMessage");
    const confirmOverlay = document.getElementById("confirmOverlay");
    const confirmText = document.getElementById("confirmText");
    const confirmYes = document.getElementById("confirmYes");
    const confirmNo = document.getElementById("confirmNo");

    confirmText.textContent = message;
    confirmMessage.style.display = "block";
    confirmOverlay.style.display = "block";

    const yesHandler = () => {
        onYes();
        hideConfirm();
    };

    const hideConfirm = () => {
        confirmMessage.style.display = "none";
        confirmOverlay.style.display = "none";
        confirmYes.removeEventListener("click", yesHandler);
        confirmNo.removeEventListener("click", hideConfirm);
        confirmOverlay.removeEventListener("click", hideConfirm);
    };

    confirmYes.addEventListener("click", yesHandler);
    confirmNo.addEventListener("click", hideConfirm);
    confirmOverlay.addEventListener("click", hideConfirm);
}
function confirmDeleteSupplier() {
    showConfirm("Are you sure you want to delete this supplier?", () => {

        document.getElementById('deleteSupplierForm').submit();

    });
}

