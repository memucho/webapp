declare var tiptapEditor: any;
declare var tiptapEditorContent: any;
declare var tiptapStarterKit: any;
declare var tiptapLink: any;
declare var tiptapCodeBlockLowlight: any;
declare var tiptapPlaceholder: any;
declare var tiptapUnderline: any;
declare var tiptapImage: any;
declare var lowlight: any;
declare var toHtml: any;

Vue.component('editor-menu-bar-component',
    {
        props: ['editor','heading'],
        template: '#editor-menu-bar-template',
        data() {
            return {
                focused: false,
                timer: null
            }
        },
        created() {
            this.editor.on('focus', () => {
                this.focused = true;
                clearTimeout(this.timer);
            });
            this.editor.on('blur', () => {
                var self = this;
                this.timer = setTimeout(() => self.focused = false, 200);
            });
        },
        methods: {
            setLink() {
                const url = window.prompt('URL');

                this.editor
                    .chain()
                    .focus()
                    .extendMarkRange('link')
                    .setLink({ href: url })
                    .run();
            },
        }
    });
