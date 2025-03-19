using System;
using System.Net;
using UnityEngine;

namespace Normcore.Services
{
    internal readonly struct Response
    {
        private readonly RequestInfo _requestInfo;
        private readonly HttpStatusCode _status;
        private readonly string _body;

        public HttpStatusCode Status => _status;
        
        internal Response(RequestInfo requestInfo, HttpStatusCode status, string body)
        {
            _requestInfo = requestInfo;
            _status = status;
            _body = body;
        }
        
        [Serializable]
        private struct DataResponse<T>
        {
            public T data;
        }
        
        [Serializable]
        private struct PagedDataResponse<T>
        {
            public T[] results;
            public int count;
            public int offset;
            public int limit;
            public string continuationToken;
        }

        /// <summary>
        /// Parse the response JSON into a data response.
        /// </summary>
        /// <typeparam name="T">The type of the "data" value in the JSON. Must be serializable.</typeparam>
        /// <returns>The "data" value of the JSON, parsed into type <typeparamref name="T"/>.</returns>
        public T ParseDataResponse<T>()
        {
            ThrowIfError();
            
            Debug.Assert(!string.IsNullOrEmpty(_body), "The response body must not be empty.");

            return JSON.Deserialize<DataResponse<T>>(_body).data;
        }
        
        /// <summary>
        /// Parse the response JSON into a page response.
        /// </summary>
        /// <typeparam name="T">The type of the "data" value in the JSON. Must be serializable.</typeparam>
        /// <returns>The "data" value of the JSON, parsed into a page of type <typeparamref name="T"/>.</returns>
        public Page<T> ParsePageResponse<T>()
        {
            ThrowIfError();

            if (_requestInfo.page == null)
            {
                throw new InvalidOperationException("The request was not paginated.");
            }
            
            Debug.Assert(!string.IsNullOrEmpty(_body), "The response body must not be empty.");

            var response = JSON.Deserialize<DataResponse<PagedDataResponse<T>>>(_body).data;
            
            var results = response.results;
            var currentPage = _requestInfo.page.Value;
            var nextPage = response.count == response.limit ? currentPage.NextPage(response.continuationToken) : null;
            
            return new Page<T>(results, currentPage, nextPage);
        }

        /// <summary>
        /// Throws an exception if the request was not successful.
        /// </summary>
        public void ThrowIfError()
        {
            // The status codes are technically overly restrictive but true for our responses.
            if (_status == HttpStatusCode.OK || _status == HttpStatusCode.NoContent)
            {
                return;
            }

            Debug.Assert(!string.IsNullOrEmpty(_body), "The response body must not be empty.");

            var error = JSON.Deserialize<Error>(_body);

            throw error switch
            {
                ValidationError argumentError => new InvalidSchemaException(_requestInfo, _status, argumentError),
                _ => error.Code switch
                {
                    ErrorCode.InternalServerError           => new InternalServerErrorException         (_requestInfo, _status, error),
                    ErrorCode.NotImplemented                => new NotImplementedException              (_requestInfo, _status, error),
                    ErrorCode.BadRequest                    => new BadRequestException                  (_requestInfo, _status, error),
                    ErrorCode.InvalidApiKey                 => new InvalidApiKeyException               (_requestInfo, _status, error),
                    ErrorCode.Unauthenticated               => new UnauthenticatedException             (_requestInfo, _status, error),
                    ErrorCode.NotPermitted                  => new NotPermittedException                (_requestInfo, _status, error),
                    ErrorCode.Nonexistent                   => new NonexistentException                 (_requestInfo, _status, error),
                    ErrorCode.LobbyNotFound                 => new LobbyNotFoundException               (_requestInfo, _status, error),
                    ErrorCode.LobbyAlreadyExists            => new LobbyAlreadyExistsException          (_requestInfo, _status, error),
                    ErrorCode.LobbyInvalidQuery             => new LobbyInvalidQueryException           (_requestInfo, _status, error),
                    ErrorCode.LobbyMemberNotFound           => new LobbyMemberNotFoundException         (_requestInfo, _status, error),
                    ErrorCode.LobbyMemberCapacityExceeded   => new LobbyMemberCapacityExceededException (_requestInfo, _status, error),
                    ErrorCode.LobbyOwnerNotFound            => new LobbyOwnerNotFoundException          (_requestInfo, _status, error),
                    ErrorCode.LobbyOwnerInvalid             => new LobbyOwnerInvalidException           (_requestInfo, _status, error),
                    ErrorCode.UserNotFound                  => new UserNotFoundException                (_requestInfo, _status, error),
                    _                                       => new ErrorResponseException               (_requestInfo, _status, error),
                },
            };
        }
    }
}
