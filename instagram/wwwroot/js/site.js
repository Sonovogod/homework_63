$.ajaxSetup({
    headers: {'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()}
});

$(document).ready(() => {
    
    $('.followButton').on('click', function (event){
        event.preventDefault();
        const followerName = $(this).attr('accountName');
        console.log(followerName);

        $.ajax({
            type: 'POST',
            url: '/Account/Follow/',
            data: { followerName: followerName },
            success: function (response){
                $('#followersCount').text(response.followerCount);
                console.log(response);
                console.log(response.followerCount)
                
                if (response.isFollow){
                    $('.btn-' + followerName).text('Отписаться')
                }
                else{
                    $('.btn-' + followerName).text('Подписаться')
                }
            },
            error: function (response) {
                console.log(response);
            }
        })
    });
    
    $('.likePost').on('click', function (event) {
        event.preventDefault();
        const postId = $(this).attr('id');
        console.log(postId);
        
        $.ajax({
            type: 'POST',
            url: '/Posts/Like/',
            data: { postId: postId},
            success: function (response){
                console.log(response);
                $('#likesCount-' + postId).text(response + ' Нравится');
            },
            error: function (response) {
                console.log(response);
            }
        })
    })
    
    $('.btnPostDelete').on('click', function (event){
        event.preventDefault();
        const postId = $(this).attr('postId');
        const postOwner = $(this).attr('accountName');
        console.log(postId);
        
        $.ajax({
            type: 'POST',
            url: '/Posts/Delete/',
            data: { postId: postId, postOwner: postOwner },
            success: function (response){
                console.log(response);
                $('#profilePost-' + postId).remove();
                $('#posts-' + accountName).text(response);
            },
            error: function (response){
                console.log(response);
            }
        })
    })

    $(document).on('click', '.editPostBtn', function(event) {
        event.preventDefault();
        const postId = $(this).attr('postId');
        const postOwner = $(this).attr('accountName');
        const saveButton = $('#savePostButton-' + postId);
        const editField = $('#profilePost-' + postId + ' textarea');

        editField.prop('disabled', false);
        saveButton.removeAttr('hidden');

        $(document).on('click', '#savePostButton-' + postId, function(event) {
            event.preventDefault();
            const content = editField.val();
            const PostEditViewModel = {
                content: content,
                postId: postId,
                postOwner: postOwner
            };

            $.ajax({
                type: 'POST',
                url: '/Posts/EditPost/',
                data: PostEditViewModel,
                success: function(response) {
                    console.log(response);
                },
                error: function(response) {
                    console.log(response);
                }
            });

            editField.prop('disabled', true);
            saveButton.attr('hidden', 'hidden');
        });
    });
})
