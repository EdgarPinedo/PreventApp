const loginRegister = document.querySelector('.login-register');
const loginLink = document.querySelector('.login-link');
const registerLink = document.querySelector('.register-link');

registerLink.addEventListener('click', () => {
    loginRegister.classList.add('active');
})

loginLink.addEventListener('click', () => {
    loginRegister.classList.remove('active');
})