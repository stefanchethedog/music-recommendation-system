const BASE_URI = "http://localhost:5252";

const GET_ALL_USERS = `${BASE_URI}/users`;
const GET_USER_BY_USERNAME = (username: String) =>
  `${BASE_URI}/users/getUserByUsername?username=${username}`;
const FOLLOW_USER = (id: String, username: String) =>
  `${BASE_URI}/users/Follow?id=${id}&username=${username}`;
const SUBSCRIBE_TO = (id: String, name: String) =>
  `${BASE_URI}/users/subscribe?id=${id}&name=${name}`;
const LIKE_SONG = (id: String, name: String) =>
  `${BASE_URI}/users/AddUserLikesSong?id=${id}&songName=${name}`;
const GET_RECOMMENDED_SONGS = (id: String) =>
  `${BASE_URI}/users/RecommendSongsByLikedSongs?id=${id}`;
const GET_LIKED_SONGS = (id: String) =>
  `${BASE_URI}/users/getLikedSongs?id=${id}`;
const CREATE_USER = `${BASE_URI}/users`;

const GET_ALL_ARTISTS = `${BASE_URI}/artists`;
const CREATE_ARTIST = `${BASE_URI}/artists`;


const GET_ALL_GENRES = `${BASE_URI}/genres`
const CREATE_GENRE = `${BASE_URI}/genres`

export {
  BASE_URI,
  GET_ALL_USERS,
  CREATE_USER,
  GET_USER_BY_USERNAME,
  FOLLOW_USER,
  SUBSCRIBE_TO,
  LIKE_SONG,
  GET_RECOMMENDED_SONGS,
  GET_LIKED_SONGS,
  GET_ALL_ARTISTS,
  CREATE_ARTIST,
  GET_ALL_GENRES,
  CREATE_GENRE
};
