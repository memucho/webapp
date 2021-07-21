Vue.component('editor-content', tiptapEditorContent);

var textComponent = Vue.component('text-component',
    {
        props: ['content'],
        data() {
            return {
                linkUrl: null,
                linkMenuIsActive: false,
                json: null,
                html: this.content,
                contentHasBeenSaved: false,
                contentIsChanged: false,
                savedContent: null,
                editor: null,
            }
        },
        created() {
            this.$root.content = this.html;
        },
        mounted() {
            this.editor = new tiptapEditor({
                content: this.content,
                extensions: [
                    tiptapStarterKit.configure({
                        heading: {
                            levels: [2, 3]
                        }
                    }),
                    tiptapLink.configure({
                        HTMLAttributes: {
                            target: '_self',
                            rel: 'noopener noreferrer nofollow'
                        }
                    }),
                    tiptapPlaceholder.configure({
                        emptyEditorClass: 'is-editor-empty',
                        emptyNodeClass: 'is-empty',
                        placeholder: 'Klicke hier um zu tippen ...',
                        showOnlyWhenEditable: true,
                        showOnlyCurrent: true,
                    }),
                    tiptapCodeBlockLowlight.configure({
                        lowlight,
                    }),
                    tiptapUnderline,
                    tiptapImage
                ],
                editorProps: {
                    handleKeyDown: (e, k) => {
                        this.contentIsChanged = true;
                    },
                    attributes: {
                        id: 'InlineEdit',
                    }
                },
                onPaste: () => {
                    this.contentIsChanged = true;
                },
                onUpdate: ({ editor }) => {
                    this.json = editor.getJSON();
                    this.html = editor.getHTML();
                },
                onFocus({ editor, event }) {
                },
                onBlur({ editor, event }) {
                },
                nativeExtensions: [
                ]
            });
            eventBus.$on('save-success',
                () => {
                    this.contentHasBeenSaved = true;
                    this.savedContent = this.html;
                });
            eventBus.$on('cancel-edit-mode',
                () => {
                    var newContent = this.contentHasBeenSaved ? this.savedContent : this.content;
                    this.contentIsChanged = false;
                    this.editor.destroy();
                    this.editor =
                        new tiptapEditor({
                            content: newContent,
                            extensions: [
                                tiptapStarterKit.configure({
                                    heading: {
                                        levels: [2, 3]
                                    }
                                }),
                                tiptapLink.configure({
                                    HTMLAttributes: {
                                        target: '_self',
                                        rel: 'noopener noreferrer nofollow'
                                    }
                                }),
                                tiptapPlaceholder.configure({
                                    emptyEditorClass: 'is-editor-empty',
                                    emptyNodeClass: 'is-empty',
                                    placeholder: 'Klicke hier um zu tippen ...',
                                    showOnlyWhenEditable: true,
                                    showOnlyCurrent: true,
                                }),
                                tiptapCodeBlockLowlight.configure({
                                    lowlight,
                                })
                            ],
                            editorProps: {
                                handleKeyDown: (e, k) => {
                                    this.contentIsChanged = true;
                                },
                            },
                            onPaste: () => {
                                this.contentIsChanged = true;
                            },
                            onUpdate: () => {
                                this.json = this.editor.getJSON();
                                this.html = this.editor.getHTML();
                            },
                            nativeExtensions: [
                            ]
                        });
                });
        },
        watch: {
            html() {
                this.$root.content = this.html;
                if (this.contentIsChanged)
                    eventBus.$emit('content-change');
            },

            json() {
                this.$root.json = this.json;
            }
        },
        methods: {
            showLinkMenu(attrs) {
                this.linkUrl = attrs.href;
                this.$nextTick(() => {
                    this.$refs.linkInput.focus();
                });
            },
            hideLinkMenu() {
                this.linkUrl = null;
                $('#inlineEditLinkModal').modal('hide');
            },
            setLinkUrl(command, url) {
                command({ href: url });
                this.hideLinkMenu();
            },
        },
    });