$(document).ready(() => {
    
    $('.followButton').on('click', function (event){
        event.preventDefault();
        const followerName = $(this).attr('accountName');
        console.log(followerName);

        $.ajax({
            type: 'GET',
            url: '/Account/Follow/',
            data: { followerName: followerName },
            success: function (response){
                $('#followersCount').text(response.followerCount);
                console.log(response)
                console.log(response.followerCount)
                
                if (response.isFollow){
                    $('.btn-' + followerName).text('Отписаться')
                }
                else{
                    $('.btn-' + followerName).text('Подписаться')
                }
            },
            error: function (response) {
                console.log(response)
            }
        })
    });
})
