document.addEventListener('DOMContentLoaded', function () {

    if (document.getElementById('category-filter')) {
        document.querySelectorAll('input[type="checkbox"][name="SelectedCategories"]').forEach(cb => {
            cb.addEventListener('change', () => {
                cb.form.submit();
            });
        });
    }

    if (document.getElementById('category-selector')) {
        document.querySelectorAll('input.btn-check').forEach(checkbox => {
            checkbox.addEventListener('change', () => {
                const label = document.querySelector(`label[for="${checkbox.id}"]`);
                if (!label) return;

                if (checkbox.checked) {
                    label.classList.remove('btn-outline-danger');
                    label.classList.add('btn-success');
                } else {
                    label.classList.remove('btn-success');
                    label.classList.add('btn-outline-danger');
                }
            });
        });

        const textarea = document.getElementById('textAreaInput');
        textarea.style.height = 'auto';
        textarea.style.height = textarea.scrollHeight + 'px';

        textarea.addEventListener('input', function () {
            this.style.height = 'auto';
            this.style.height = this.scrollHeight + 'px';
        });
    }
});
