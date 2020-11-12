const Notification = {
    init() {
        this.hideTimeout = null;

        this.el = document.createElement('div');
        this.el.className = 'notification';
        document.body.appendChild(this.el);
    },

    show(message, state) {
        clearTimeout(this.hideTimeout);

        this.el.textContent = message;
        this.el.className = 'notification notification--visible';

        if (state) {
            this.el.classList.add(`notification--${state}`)
        }

        this.hideTimeout = setTimeout(() => {
            this.el.classList.remove('notification--visible');
        }, 3000);
    }
};

document.addEventListener('DOMContentLoaded', () => Notification.init());