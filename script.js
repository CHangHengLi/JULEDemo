document.addEventListener('DOMContentLoaded', () => {
    const characterCards = document.querySelectorAll('.character-card');

    characterCards.forEach(card => {
        card.addEventListener('click', () => {
            const characterName = card.querySelector('p').textContent;
            alert(`你点击了：${characterName}`);
        });
    });

    // You can add more JavaScript interactions here if needed.
    console.log('JavaScript file loaded and executed.');
});
