import React, { useEffect, useState } from 'react';
import { isInteger, isEmptyOrSpaces } from '../../../Utils/Utils';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { decode } from '../../../Services/Methods';
import { Link, useParams, Navigate } from 'react-router-dom';
import { addOrUpdatePost, getFilter, getPostById } from '../../../Services/BlogRepository';

const Edit = () => {
  const initialState = {
      id: 0,
      title: '',
      shortDescription: '',
      description: '',
      urlSlug: '',
      meta: '',
      imageUrl: '',
      category: {},
      author: {},
      tags: [],
      selectedTags: '',
      published: false,
    },
    [post, setPost] = useState(initialState),
    [filter, setFilter] = useState({
      authorList: [],
      categoryList: [],
    });

  let { id } = useParams();
  id = id ?? 0;

  useEffect(() => {
    document.title = 'Thêm/Cập nhật bài viết';

    getPostById(id).then((data) => {
      console.log(data);
      if (data)
        setPost({
          ...data,
          selectedTags: data.tags.map((tag) => tag?.name).join('\r\n'),
        });
      else setPost(initialState);
    }, []);

    getFilter().then((data) => {
      if (data)
        setFilter({
          authorList: data.authorList,
          categoryList: data.categoryList,
        });
      else
        setFilter({
          authorList: [],
          categoryList: [],
        });
    });
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    let form = new FormData(e.target);

    addOrUpdatePost(form).then((data) => {
      if (data) alert('Lưu thành công!');
      else alert('Đã xảy ra lỗi!!');
    });
  };

  if (id && !isInteger(id)) return <Navigate to={`/400?redirectTo=/admin/posts`} />;
  return (
    <>
      <Form
        method='post'
        encType='multipart/form-data'
        onSubmit={handleSubmit}
      >
        <Form.Control
          type='hidden'
          name='id'
          value={post.id}
        />
        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Tiêu đề</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='text'
              name='title'
              title='Title'
              required
              value={post.title || ''}
              onChange={(e) =>
                setPost({
                  ...post,
                  title: e.target.value,
                })
              }
            />
          </div>
        </div>
        {/* <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Slug</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='text'
              name='urlSlug'
              title='Url Slug'
              value={post.urlSlug || ''}
              onChange={(e) =>
                setPost({
                  ...post,
                  urlSlug: e.target.value,
                })
              }
            />
          </div>
        </div> */}
        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Giới thiệu</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              as={'textarea'}
              type='text'
              required
              name='description'
              title='description'
              value={decode(post.description || '')}
              onChange={(e) =>
                setPost({
                  ...post,
                  description: e.target.value,
                })
              }
            />
          </div>
        </div>
        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Giới thiệu ngắn</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              as={'textarea'}
              type='text'
              required
              name='shortDescription'
              title='Short Description'
              value={decode(post.shortDescription || '')}
              onChange={(e) =>
                setPost({
                  ...post,
                  shortDescription: e.target.value,
                })
              }
            />
          </div>
        </div>
        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Metadata</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='text'
              required
              name='meta'
              title='Meta'
              value={decode(post.meta || '')}
              onChange={(e) =>
                setPost({
                  ...post,
                  meta: e.target.value,
                })
              }
            />
          </div>
        </div>
        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Tác giả</Form.Label>
          <div className='col-sm-10'>
            <Form.Select
              required
              name='authorId'
              title='Author Id'
              value={post.author.id}
              onChange={(e) =>
                setPost({
                  ...post,
                  author: e.target.value,
                })
              }
            >
              <option value={''}>-- Chọn tác giả --</option>
              {filter.authorList.length > 0 &&
                filter.authorList.map((item, index) => (
                  <option
                    key={index}
                    value={item.value}
                  >
                    {item.text}
                  </option>
                ))}
            </Form.Select>
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Chủ đề</Form.Label>
          <div className='col-sm-10'>
            <Form.Select
              required
              name='categoryId'
              title='Category Id'
              value={post.category.id}
              onChange={(e) =>
                setPost({
                  ...post,
                  category: e.target.value,
                })
              }
            >
              <option value={''}>-- Chọn chủ đề --</option>
              {filter.categoryList.length > 0 &&
                filter.categoryList.map((item, index) => (
                  <option
                    key={index}
                    value={item.value}
                  >
                    {item.text}
                  </option>
                ))}
            </Form.Select>
          </div>
        </div>
        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Từ khóa (mỗi thẻ 1 dòng)</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              as={'textarea'}
              rows={5}
              type='text'
              required
              name='selectedTags'
              title='Selected Tags'
              value={post.selectedTags || ''}
              onChange={(e) =>
                setPost({
                  ...post,
                  selectedTags: e.target.value,
                })
              }
            />
          </div>
        </div>
        {!isEmptyOrSpaces(post.imageUrl) && (
          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-label'>Hình hiện tại</Form.Label>
            <div className='col-sm-10'>
              <img
                src={`https://localhost:7251/${post.imageUrl}`}
                alt={post.title}
              />
            </div>
          </div>
        )}
        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Chọn hình ảnh</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='file'
              name='imageFile'
              accept='image/*'
              title='Image File'
              onChange={(e) =>
                setPost({
                  ...post,
                  imageFile: e.target.files[0],
                })
              }
            />
          </div>
        </div>
        <div className='row mb-3'>
          <div className='col-sm-10 offset-sm-2'>
            <div className='form-check'>
              <input
                className='form-check-input'
                type='checkbox'
                name='published'
                checked={post.published}
                value={post.published}
                title='Published'
                onChange={(e) =>
                  setPost({
                    ...post,
                    published: e.target.checked,
                  })
                }
              />
              <Form.Label className='form-check-label'>Đã xuất bản</Form.Label>
            </div>
          </div>
        </div>
        <div className='text-center'>
          <Button
            variant='primary'
            type='submit'
          >
            Lưu thay đổi
          </Button>
          <Link
            to={`/admin/posts`}
            className='btn btn-danger ms-2'
          >
            Hủy và quay lại
          </Link>
        </div>
      </Form>
    </>
  );
};

export default Edit;
